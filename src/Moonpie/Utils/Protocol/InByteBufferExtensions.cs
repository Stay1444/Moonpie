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
using System.Text.Json;
using Moonpie.Protocol.Network;
using Moonpie.Protocol.Protocol;

namespace Moonpie.Utils.Protocol;

public static class InByteBufferExtensions
{
    public static NBT? ReadNbt(this InByteBuffer buffer)
    {
        return ReadNbt(buffer, buffer.Version < ProtocolVersion.v14w28b);
    }

    private static NBT? ReadNbt(InByteBuffer buffer, bool compressed)
    {
        if (compressed)
        {
            //ToDo
            return null;
        }
        else
        {
            var type = (NBTTagTypes) buffer.ReadByte();
            if (type == NBTTagTypes.Compound)
            {
                var name = buffer.ReadString(buffer.ReadUShort()); // ToDo
            }

            return ReadNBTTag(buffer, type);
        }
    }

    private static NBT? ReadNBTTag(InByteBuffer buffer, NBTTagTypes type)
    {
        switch (type)
        {
            case NBTTagTypes.End:
                return null;
            case NBTTagTypes.Byte:
                return new NBT(type, buffer.ReadByte());
            case NBTTagTypes.Short:
                return new NBT(type, buffer.ReadShort());
            case NBTTagTypes.Int:
                return new NBT(type, buffer.ReadInt());
            case NBTTagTypes.Long:
                return new NBT(type, buffer.ReadLong());
            case NBTTagTypes.Float:
                return new NBT(type, buffer.ReadFloat());
            case NBTTagTypes.Double:
                return new NBT(type, buffer.ReadDouble());
            case NBTTagTypes.ByteArray:
                return new NBT(type, buffer.ReadBytes(buffer.ReadInt()));
            case NBTTagTypes.String:
                return new NBT(type, buffer.ReadString(buffer.ReadUShort()));
            case NBTTagTypes.List:
            {
                var listType = (NBTTagTypes) buffer.ReadByte();
                var listSize = buffer.ReadInt();
                var list = new List<NBT?>();
                for (int i = 0; i < listSize; i++)
                {
                    list.Add(ReadNBTTag(buffer, listType));
                }
                return new NBT(type, list);
            }
            case NBTTagTypes.Compound:
            {
                var dict = new Dictionary<string, NBT?>();
                while (true)
                {
                    var compoundTagType = (NBTTagTypes) buffer.ReadByte();
                    if (compoundTagType == NBTTagTypes.End)
                    {
                        break;
                    }
                    var compoundTagName = buffer.ReadString(buffer.ReadUShort());
                    var tag = ReadNBTTag(buffer, compoundTagType);
                    if (tag is null) continue;
                    dict.Add(compoundTagName, tag);
                }
                return new NBT(type, dict);
            }
            case NBTTagTypes.IntArray:
            {
                var listSize = buffer.ReadInt();
                var list = new List<int>();
                for (int i = 0; i < listSize; i++)
                {
                    list.Add(buffer.ReadInt());
                }
                return new NBT(type, list);
            }
            case NBTTagTypes.LongArray:
            {
                var listSize = buffer.ReadInt();
                var list = new List<long>();
                for (int i = 0; i < listSize; i++)
                {
                    list.Add(buffer.ReadLong());
                }
                return new NBT(type, list);
            }
            default:
                throw new Exception("Unknown NBT type: " + type);
        }
    }
    
    public static JavaUUID ReadUUIDString(this InByteBuffer buffer)
    {
        string uuid = buffer.ReadString().Trim();
        if (uuid.Length == 36)
        {
            return JavaUUID.FromString(uuid);
        }

        if (uuid.Length == 32)
        {
            // uuid comes in as a string of 32 hex characters, but the Java UUID class expects it in 8-4-4-4-12 format
            string uuidWithDashes = uuid.Substring(0, 8) + "-" + uuid.Substring(8, 4) + "-" + uuid.Substring(12, 4) + "-" + uuid.Substring(16, 4) + "-" + uuid.Substring(20, 12);
            return JavaUUID.FromString(uuidWithDashes);
        }
        
        throw new Exception("Invalid UUID string: " + uuid);
    }
    
    public static JavaUUID ReadUUID(this InByteBuffer buffer)
    {
        var bytes = buffer.ReadBytes(16);
        return new JavaUUID(bytes);
    }
    
    public static T? ReadJson<T>(this InByteBuffer buffer)
    {
        string json = buffer.ReadString();
        return JsonSerializer.Deserialize<T>(json);
    }
    
    public static ChatComponent ReadChatComponent(this InByteBuffer buffer)
    {
        string json = buffer.ReadString();
        return JsonSerializer.Deserialize<ChatComponent>(json)!;
    }
    public static T? ReadOptional<T>(this InByteBuffer buffer, Func<T> action)
    {
        if (buffer.ReadBoolean())
        {
            return action();
        }
        return default;
    }
    
    public static T[] ReadArray<T>(this InByteBuffer buffer, int length, Func<T> read)
    {
        var array = new T[length];
        for (int i = 0; i < length; i++)
        {
            array[i] = read();
        }
        return array;
    }

    public static T[] ReadArray<T>(this InByteBuffer buffer, Func<InByteBuffer, T> read, int length)
    {
        var array = new T[length];
        for (int i = 0; i < length; i++)
        {
            array[i] = read(buffer);
        }
        return array;
    }
    
    public static PlayerProperties ReadPlayerProperties(this InByteBuffer buffer)
    {
        PlayerTextures? textures = null;
        int length = buffer.ReadVarInt();
        for (int i = 0; i < length; i++)
        {
            var name = buffer.ReadString();
            var value = buffer.ReadString();
            string? signature = null;
            if (buffer.Version < ProtocolVersion.v14w21a)
            {
                signature = buffer.ReadString();
            }
            else
            {
                signature = buffer.ReadOptional(buffer.ReadString);
            }

            if (name == "textures")
            {
                if (textures is not null) throw new Exception("Duplicate textures property");
                textures = PlayerTextures.From(value, signature ?? throw new Exception("Missing signature"));
            }
            else
            {
                throw new Exception("Unknown player property: " + name);
            }
        }
        return new PlayerProperties()
        {
            Textures = textures
        };
    }
}