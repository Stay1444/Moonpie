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
using Moonpie.Utils.Math;

namespace Moonpie.Utils.Protocol;

public static class InByteBufferExtensions
{
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
        var json = buffer.ReadString();
        return ChatComponent.FromJson(json);
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
    
    public static Vector3d ReadVector3d(this InByteBuffer buffer)
    {
        return new Vector3d(buffer.ReadDouble(), buffer.ReadDouble(), buffer.ReadDouble());
    }
    
}