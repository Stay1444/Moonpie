using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using Moonpie.Protocol.Network;
using Moonpie.Protocol.Protocol;

namespace Moonpie.Utils.Protocol;

public static class OutByteBufferExtensions
{
    public static void WriteChatComponent(this OutByteBuffer buffer, ChatComponent c)
    {
        string json = JsonSerializer.Serialize(c);
        buffer.WriteString(json);
    }
    
    public static void WriteJson(this OutByteBuffer buffer, object v)
    {
        string json = JsonSerializer.Serialize(v);
        buffer.WriteString(json);
    }
    
    public static void WriteUUIDString(this OutByteBuffer buffer, JavaUUID guid)
    {
        buffer.WriteString(guid.ToString().Replace("-", ""));
    }
    
    public static void WriteUUID(this OutByteBuffer buffer, JavaUUID? guid)
    {
        if (guid is not null)
        {
            buffer.WriteLong(guid.Value.MostSigBits);
            buffer.WriteLong(guid.Value.LeastSigBits);
        }
        else
        {
            buffer.WriteLong(0);
            buffer.WriteLong(0);
        }
    }
    
    public static void WriteNbt(this OutByteBuffer buffer, NBT? obj)
    {
        WriteNbt(buffer, obj, buffer.Version < ProtocolVersion.v14w28b);
    }
    
    private static void WriteNbt(OutByteBuffer buffer, NBT? data, bool compressed)
    {
        if (compressed)
        {
            //Todo
        }
        else
        {
            if (data?.Value is List<object?> list)
            {
                if (!list.Any())
                {
                    WriteNBTTag(buffer, null);
                    return;
                }

                WriteNBTTagType(buffer, NBTTagTypes.Compound);
                buffer.WriteShort(0);
                WriteNBTTag(buffer, data, false);
            }

            WriteNBTTag(buffer, data);
        }
    }
    
    private static void WriteNBTTagType(OutByteBuffer buffer, NBTTagTypes t)
    {
        buffer.WriteByte((byte)t);
    }
    
    private static void WriteNBTTag(OutByteBuffer buffer, NBT? data, bool writeType = true)
    {
        void WriteTagType(NBTTagTypes t)
        {
            if (!writeType)
            {
                return;
            }

            WriteNBTTagType(buffer, t);
        }
        
        WriteTagType(data!.Type);
        if (data.Type == NBTTagTypes.End)
        {
            return;
        }

        switch (data.Type)
        {
            case NBTTagTypes.End:
                return;
            case NBTTagTypes.Byte:
                buffer.WriteByte((byte)data.Value!);
                break;
            case NBTTagTypes.Short:
                buffer.WriteShort((short)data.Value!);
                break;
            case NBTTagTypes.Int:
                buffer.WriteInt((int)data.Value!);
                break;
            case NBTTagTypes.Long:
                buffer.WriteLong((long)data.Value!);
                break;
            case NBTTagTypes.Float:
                buffer.WriteFloat((float)data.Value!);
                break;
            case NBTTagTypes.Double:
                buffer.WriteDouble((double)data.Value!);
                break;
            case NBTTagTypes.ByteArray:
            {
                var bytes = (byte[])data.Value!;
                buffer.WriteInt(bytes.Length);
                buffer.WriteBytes(bytes);
                break;
            }
            case NBTTagTypes.String:
            {
                var stringBytes = Encoding.UTF8.GetBytes((string)data.Value!);
                if (stringBytes.Length > ushort.MaxValue)
                {
                    throw new ArgumentException("String too long");
                }
                buffer.WriteUShort((ushort) stringBytes.Length);
                buffer.WriteBytes(stringBytes);
                break;
            }
            case NBTTagTypes.List:
            {
                var list = (List<NBT?>)data.Value!;
                if (list.Count == 0)
                {
                    WriteNBTTagType(buffer, NBTTagTypes.End);
                    return;
                }

                WriteNBTTagType(buffer, list.First()!.Type);
                buffer.WriteInt(list.Count);
                foreach (var item in list)
                {
                    WriteNbt(buffer, item, false);
                }
                break;
            }
            case NBTTagTypes.Compound:
            {
                var compound = (Dictionary<string, NBT?>)data.Value!;
                foreach (var (key, value) in compound)
                {
                    if (value!.Type == NBTTagTypes.End)
                    {
                        throw new ArgumentException("Compound cannot contain End tag");
                    }
                    WriteNBTTagType(buffer, value.Type);
                    WriteNBTTag(buffer, new NBT(NBTTagTypes.String, key), false);
                    WriteNbt(buffer, value, false);
                }
                WriteNBTTagType(buffer, NBTTagTypes.End);
                break;
            }
            case NBTTagTypes.IntArray:
            {
                var intArray = (int[])data.Value!;
                buffer.WriteInt(intArray.Length);
                foreach (var i in intArray)
                {
                    buffer.WriteInt(i);
                }
                break;
            }
            case NBTTagTypes.LongArray:
            {
                var longArray = (long[])data.Value!;
                buffer.WriteInt(longArray.Length);
                foreach (var i in longArray)
                {
                    buffer.WriteLong(i);
                }
                break;
            }
        }
    }

    public static void WritePlayerProperties(this OutByteBuffer buffer, PlayerProperties properties)
    {
        buffer.WriteVarInt(0);
    }
    
    public static void WriteArray<T>(this OutByteBuffer buffer, IEnumerable<T> enumerable, Action<T> write)
    {
        foreach (var item in enumerable)
        {
            write(item);
        }
    }
}