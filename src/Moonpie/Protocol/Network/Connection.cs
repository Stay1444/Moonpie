#region License
// Moonpie
// 
// Copyright (c) 2022 Stay
// 
// MIT License
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
#endregion

using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Moonpie.Protocol.Packets;
using Moonpie.Protocol.Packets.c2s;
using Moonpie.Protocol.Packets.s2c;
using Moonpie.Protocol.Protocol;
using Moonpie.Utils.Protocol;
using Serilog;

namespace Moonpie.Protocol.Network;

public abstract class Connection : IDisposable
{
    private const bool LOG = false;
    private TcpClient _client;
    private NetworkStream _stream;
    private PacketMapper _packetMapper;
    public bool IsConnected { get; private set; }

    public bool CompressionEnabled
    {
        get => CompressionThreshold > 0;
    }
    public long CompressionThreshold { get; internal set; }
    public EndPoint RemoteEndPoint { get; private protected set; }
    public ProtocolVersion Version { get; internal set; }
    public ProtocolState State { get; internal set; }
    private CancellationTokenSource _cts;
    public event EventHandler? Disconnected;

    internal Connection(TcpClient client)
    {
        _cts = new CancellationTokenSource();
        _client = client;
        _stream = client.GetStream();
        RemoteEndPoint = client.Client.RemoteEndPoint ?? throw new Exception("RemoteEndPoint is null");
        _packetMapper = new PacketMapper();
        IsConnected = true;
        CompressionThreshold = -1;
        Version = ProtocolVersion.v1_7_1_pre;
    }

    protected async Task<IC2SPacket?> ReadC2SPacketAsync()
    {
        if (!IsConnected || _cts.IsCancellationRequested)
        {
            return null;
        }

        try
        {

            var data = await ReadDataChunk();
            if (LOG) Log.Debug("ReadC2SPacketAsync Chunk: {data}", data.Length);
            if (data.Length == 0)
            {
                if (LOG) Log.Debug("ReadC2SPacketAsync: Disconnected");
                await DisconnectAsync();
                return null;
            }

            if (CompressionEnabled)
            {
                if (LOG) Log.Debug("ReadC2SPacketAsync: CompressionEnabled");
                data = Decompress(data);
            }

            var buffer = new InByteBuffer(data, Version, State);
            return _packetMapper.DeserializeC2SPacket(buffer);

        }
        catch (OperationCanceledException)
        {
            return null;
        }
        catch(Exception e)
        {
            Console.WriteLine(e);
            Log.Error(e, "Error reading packet C2S");
            await DisconnectAndCleanupAsync();
        }
        await DisconnectAndCleanupAsync();
        return null;
    }

    protected async Task<IS2CPacket?> ReadS2CPacketAsync()
    {
        if (!IsConnected || _cts.IsCancellationRequested)
        {
            return null;
        }

        try
        {
            
            var data = await ReadDataChunk();
            if (CompressionEnabled)
            {
                data = Decompress(data);
            }
            
            var buffer = new InByteBuffer(data, Version, State);
            var packet =  _packetMapper.DeserializeS2CPacket(buffer);

            if (packet is GenericS2CP {Type: PacketTypes.S2C.PLAY_CHUNK_DATA})
            {
                
            }
            
            return packet;

        }catch (OperationCanceledException)
        {
            return null;
        }
        catch(Exception e)
        {
            Console.WriteLine(e);
            Log.Error(e, "Error reading packet S2C");
            await DisconnectAndCleanupAsync();
        }
        
        await DisconnectAndCleanupAsync();
        return null;
    }
    
    private List<byte> _previousBytes = new List<byte>();
    byte[] _readBuffer = new byte[8192];
    private async Task<byte[]> ReadDataChunk()
    {
        if (!IsConnected || _cts.IsCancellationRequested)
        {
            return Array.Empty<byte>();
        }
        
        bool Validate(byte[] data, out byte[] result, out int readLength)
        {
            if (data.Length < 1)
            {
                result = Array.Empty<byte>();
                readLength = 0;
                return false;
            }
            
            VarInt length = new VarInt(data);
            if (length.Size + length.Value > data.Length)
            {
                result = Array.Empty<byte>();
                readLength = 0;
                return false;
            }
            
            result = new byte[length.Value];
            readLength = length.Size + length.Value;
            Array.Copy(data, length.Size, result, 0, length.Value);
            return true;
        }
        
        var buffer = new List<byte>();

        while (IsConnected && !_cts.IsCancellationRequested)
        {
            {
                if (Validate(_previousBytes.ToArray(), out var data, out var readLength))
                {
                    _previousBytes.RemoveRange(0, readLength);
                    return data;
                }
            }

            int read = await _stream.ReadAsync(_readBuffer, 0, _readBuffer.Length, _cts.Token);
            if (read == 0)
            {
                await DisconnectAndCleanupAsync();
                return Array.Empty<byte>();
            }
            buffer.AddRange(_previousBytes);
            _previousBytes.Clear();
            buffer.AddRange(_readBuffer.Take(read));
            {
                if (Validate(buffer.ToArray(), out var data, out var readLength))
                {
                    buffer.RemoveRange(0, readLength);
                    if (buffer.Count > 0)
                    {
                        _previousBytes.AddRange(buffer);
                    }
                    return data;
                }
            }
        }
        buffer.Clear();
        return Array.Empty<byte>();
    }

    private byte[] Decompress(byte[] compressedPacket)
    {
        if (!CompressionEnabled) throw new Exception("Compression is not enabled");
        
        //Get the uncompressed packet length
        var dataLength = new VarInt(compressedPacket);

        if (dataLength == 0) // Packet is below the compression threshold, data is uncompressed.
        {
            var uncompressed = new byte[compressedPacket.Length - dataLength.Size];
            Array.Copy(compressedPacket, dataLength.Size, uncompressed, 0, uncompressed.Length);
            return uncompressed;
        }

        var compressedData = new byte[compressedPacket.Length - dataLength.Size];
        
        Array.Copy(compressedPacket, dataLength.Size, compressedData, 0, compressedData.Length);

        var decompressed = ProtocolCompression.Decompress(compressedData);

        return decompressed;

    }
    
    private async Task DisconnectAndCleanupAsync()
    {
        if (!IsConnected) return;
        IsConnected = false;
        _cts.Cancel();
        await _stream.DisposeAsync();
        _client.Dispose();
        IsConnected = false;
        try
        {
            Disconnected?.Invoke(this, new EventArgs());   
        }catch(Exception e)
        {
            Console.WriteLine(e);
        }
    }
    
    public Task DisconnectAsync()
    {
        return DisconnectAndCleanupAsync();
    }

    public Task WritePacketAsync(IPacket packet)
    {
        if (!IsConnected) return Task.CompletedTask;
        
        if (packet is IS2CPacket s2c)
        {
            return WriteS2CPacket(s2c);
        }else if (packet is IC2SPacket c2s)
        {
            return WriteC2SPacket(c2s);
        }else
        {
            throw new Exception("Invalid packet type");
        }
    }

    private async Task WriteS2CPacket(IS2CPacket packet)
    {
        var stdId = _packetMapper.GetS2CPacket(packet);
        
        if (stdId is null) throw new Exception("Packet is not registered");

        var buffer = new OutByteBuffer(Version, State);
        var vPacketId = _packetMapper.GetPacketId(stdId.Value, Version);
        
        if (vPacketId is null) throw new Exception("Packet is not registered");
        
        buffer.WriteVarInt(vPacketId.Value);
        
        packet.Write(buffer);

        var packetBytes = buffer.Data;

        var serialized = SerializePacketData(packetBytes);

        await _stream.WriteAsync(serialized, 0, serialized.Length, _cts.Token);
    }
    
    private async Task WriteC2SPacket(IC2SPacket packet)
    {
        var stdId = _packetMapper.GetC2SPacket(packet);   
        
        if (stdId is null) throw new Exception("Packet is not registered");
        
        var buffer = new OutByteBuffer(Version, State);
        var vPacketId = _packetMapper.GetPacketId(stdId.Value, Version);
        
        if (vPacketId is null) throw new Exception("Packet is not registered");
        
        buffer.WriteVarInt(vPacketId.Value);
        
        packet.Write(buffer);
        
        var packetBytes = buffer.Data;
        
        var serialized = SerializePacketData(packetBytes);
        
        await _stream.WriteAsync(serialized, 0, serialized.Length, _cts.Token);
    }

    private byte[] SerializePacketData(byte[] packetData)
    {
        var buffer = new List<byte>();
        if (CompressionEnabled && packetData.Length >= CompressionThreshold)
        {
            buffer.AddRange(ProtocolCompression.Compress(packetData));
        }
        else
        {
            buffer.AddRange(packetData);
        }

        if (CompressionEnabled)
        {
            if (packetData.Length < CompressionThreshold)
            {
                buffer.InsertRange(0, new VarInt(0).ToBytes());
            }
            else
            {
                buffer.InsertRange(0, new VarInt(packetData.Length).ToBytes());
            }
        }
        
        buffer.InsertRange(0, new VarInt(buffer.Count).ToBytes());
        return buffer.ToArray();
    }
    
    

    public void Dispose()
    {
        DisconnectAndCleanupAsync().Wait();
    }
}