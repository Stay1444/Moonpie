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

using Ionic.Zlib;
using CompressionLevel = Ionic.Zlib.CompressionLevel;
using CompressionMode = Ionic.Zlib.CompressionMode;

namespace Moonpie.Utils.Protocol;

public static class ProtocolCompression
{
    public static byte[] Compress(byte[] data)
    {
        using MemoryStream memoryStream = new MemoryStream();
        Stream compressor = new ZlibStream(memoryStream, CompressionMode.Compress, CompressionLevel.Default);
        compressor.Write(data, 0, data.Length);
        compressor.Flush();
        memoryStream.Flush();
        compressor.Close();
        return memoryStream.ToArray();
    }
    
    public static byte[] Decompress(byte[] data)
    {
        using MemoryStream memoryStream = new MemoryStream(data);
        using Stream decompressor = new ZlibStream(memoryStream, CompressionMode.Decompress, CompressionLevel.Default);
        var buffer = new byte[1024];
        var result = new List<byte>();
        while (true)
        {
            var read = decompressor.Read(buffer, 0, buffer.Length);
            if (read == 0)
            {
                break;
            }
            result.AddRange(buffer.Take(read));
        }
        decompressor.Flush();
        memoryStream.Flush();
        decompressor.Close();
        var d = result.ToArray();
        return d;
    }
}