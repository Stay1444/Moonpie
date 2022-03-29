#region Copyright
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
using System.Threading.Tasks;
using Moonpie.Entities;
using Moonpie.Protocol.Network;
using Moonpie.Protocol.Protocol;
using Moonpie.Utils.Protocol;

namespace Moonpie.Protocol.Packets.s2c.Play;

[PacketType(PacketTypes.S2C.PLAY_DECLARE_COMMANDS)]
public class DeclareCommandsS2CP : IS2CPacket
{
    public enum NodeType
    {
        Root,
        Literal,
        Argument
    }
    
    public abstract class CommandNode
    {
        public bool IsExecutable { get; set; }
        public List<CommandArgumentNode> Arguments { get; set; } = new();
        public Dictionary<string, CommandLiteralNode> Literals { get; set; } = new();
        public int RedirectNodeId { get; set; }
        public List<int> ChildrenIds { get; set; } = new();
        public CommandNode? RedirectNode;
        public byte Flags { get; set; }
        public CommandNode(byte flags, InByteBuffer buffer)
        {
            this.Flags = flags;
            this.IsExecutable = flags.IsBitMask(0x04);
            this.ChildrenIds.AddRange(buffer.ReadArray(buffer.ReadVarInt(), buffer.ReadVarInt));
            if (flags.IsBitMask(0x08))
            {
                this.RedirectNodeId = buffer.ReadVarInt();
            }else
            {
                this.RedirectNodeId = -1;
            }
        }

        public CommandNode(byte flags, bool executable, params CommandNode[] children)
        {
            this.IsExecutable = executable;
            this.RedirectNodeId = -1;
            this.Flags = flags;
            if (executable)
            {
                this.Flags = flags.SetBitMask(0x04);
            }
            AddChildren(children);
        }

        public void Write(OutByteBuffer buffer)
        {
            buffer.WriteByte(Flags);
            buffer.WriteVarInt(ChildrenIds.Count);
            foreach (var child in ChildrenIds)
            {
                buffer.WriteVarInt(child);
            }
            
            if (Flags.IsBitMask(0x08))
            {
                buffer.WriteVarInt(RedirectNodeId);
            }
            
            WriteChildren(buffer);
        }

        protected abstract void WriteChildren(OutByteBuffer buffer);

        private void AddChildren(CommandNode[] children)
        {
            foreach (var node in children)
            {
                if (node is CommandArgumentNode argumentNode)
                {
                    this.Arguments.Add(argumentNode);
                    continue;
                }

                if (node is CommandLiteralNode literalNode)
                {
                    Literals.Add(literalNode.Name, literalNode);
                }
            }
        }
    }

    public class CommandRootNode : CommandNode
    {
        public CommandRootNode(byte flags, InByteBuffer buffer) : base(flags, buffer)
        {
            
        }
        
        public CommandRootNode(params CommandNode[] children) : base((byte) 0x00.SetBitMask(0), false, children)
        {
            
        }
        
        protected override void WriteChildren(OutByteBuffer buffer)
        {
            
        }
    }

    public class CommandLiteralNode : CommandNode
    {
        public string Name { get; set; }

        public CommandLiteralNode(byte flags, InByteBuffer buffer) : base(flags, buffer)
        {
            this.Name = buffer.ReadString();
        }

        public CommandLiteralNode(byte flags, string name, bool executable) : base(flags, executable)
        {
            this.Name = name;
        }

        public static CommandLiteralNode Create(string name, bool executable)
        {
            return new CommandLiteralNode((byte) 0x00.SetBitMask(1), name, executable);
        }

        protected override void WriteChildren(OutByteBuffer buffer)
        {
            buffer.WriteString(Name);
        }
    }

    public class CommandArgumentNode : CommandNode
    {
        public enum SuggestionTypes {
            ASK_SERVER,
            ALL_RECIPES,
            AVAILABLE_SOUNDS,
            SUMMONABLE_ENTITIES,
            AVAILABLE_BIOMES
        }
    
        public string Name { get; set; }
        public string Parser { get; set; }
        public SuggestionTypes SuggestionType { get; set; }
        public List<object> Data { get; set; }
        public CommandArgumentNode(byte flags, InByteBuffer buffer) : base(flags, buffer)
        {
            Name = buffer.ReadString();
            Parser = buffer.ReadString(); 
            List<object> data = new();
            
            switch (Parser)
            {
                case "brigadier:bool":
                    break;
                case "brigadier:double":
                {
                    var pFlag = buffer.ReadByte();
                    data.Add(pFlag);

                    if (pFlag.IsBitMask(0x01))
                    {
                        var doubleMin = buffer.ReadDouble();
                        data.Add(doubleMin);
                    }
                    else
                    {
                        //data.Add(double.MinValue);
                    }

                    if (pFlag.IsBitMask(0x02))
                    {
                        var doubleMax = buffer.ReadDouble();
                        data.Add(doubleMax);
                    }else
                    {
                        //data.Add(double.MaxValue);
                    }
                    break;
                }
                case "brigadier:float":
                {
                    var pFlag = buffer.ReadByte();
                    data.Add(pFlag);

                    if (pFlag.IsBitMask(0x01))
                    {
                        var floatMin = buffer.ReadFloat();
                        data.Add(floatMin);
                    }
                    else
                    {
                        //data.Add(float.MinValue);
                    }
                    
                    if (pFlag.IsBitMask(0x02))
                    {
                        var floatMax = buffer.ReadFloat();
                        data.Add(floatMax);
                    }else
                    {
                        //data.Add(float.MaxValue);
                    }
                    
                    break;
                }
                case "brigadier:integer":
                {
                    var pFlag = buffer.ReadByte();
                    data.Add(pFlag);
                    if (pFlag.IsBitMask(0x01))
                    {
                        var intMin = buffer.ReadInt();
                        data.Add(intMin);
                    }
                    else
                    {
                        //data.Add(int.MinValue);
                    }
                    
                    if (pFlag.IsBitMask(0x02))
                    {
                        var intMax = buffer.ReadInt();
                        data.Add(intMax);
                    }else
                    {
                        //data.Add(int.MaxValue);
                    }
                    
                    break;
                }
                case "brigadier:string":
                {
                    var l = buffer.ReadVarInt();
                    data.Add(new VarInt(l));
                    break;
                }
                case "minecraft:entity":
                {
                    data.Add(buffer.ReadByte());
                    break;
                }
                case "minecraft:range":
                {
                    data.Add(buffer.ReadByte());
                    break;
                }
                case "minecraft:score_holder":
                {
                    data.Add(buffer.ReadByte());
                    break;
                }
                default:
                    break;
            }
            Data = data;

            if ((flags & 0x10) != 0)
            {
                var suggestionResource = buffer.ReadString();
                SuggestionType = suggestionResource switch
                {
                    "minecraft:ask_server" => SuggestionTypes.ASK_SERVER,
                    "minecraft:all_recipes" => SuggestionTypes.ALL_RECIPES,
                    "minecraft:available_sounds" => SuggestionTypes.AVAILABLE_SOUNDS,
                    "minecraft:summonable_entities" => SuggestionTypes.SUMMONABLE_ENTITIES,
                    "minecraft:available_biomes" => SuggestionTypes.AVAILABLE_BIOMES,
                    _ => throw new ArgumentOutOfRangeException(nameof(suggestionResource), suggestionResource, null)
                };
            }

        }

        public CommandArgumentNode(byte flags, bool executable, string name, string parser, SuggestionTypes suggestionType,
            List<object> data) : base(flags, executable, Array.Empty<CommandNode>())
        {
            Name = name;
            Parser = parser;
            SuggestionType = suggestionType;
            Data = data;
        }
        
        protected override void WriteChildren(OutByteBuffer buffer)
        {
            buffer.WriteString(Name);
            buffer.WriteString(Parser);
            foreach (var thing in Data)
            {
                if (thing is byte b)
                {
                    buffer.WriteByte(b);
                }else if (thing is short s)
                {
                    buffer.WriteShort(s);
                }else if (thing is int i)
                {
                    buffer.WriteInt(i);
                }else if (thing is float f)
                {
                    buffer.WriteFloat(f);
                }else if (thing is string st)
                {
                    buffer.WriteString(st);
                }else if (thing is long l)
                {
                    buffer.WriteLong(l);
                }else if (thing is double d)
                {
                    buffer.WriteDouble(d);
                }else if (thing is VarInt varint)
                {
                    buffer.WriteVarInt(varint.Value);
                }
                else
                {
                    throw new Exception("Unknown type");
                }
            }

            if ((Flags & 0x10) != 0)
            {
                switch (SuggestionType)
                {
                    case SuggestionTypes.ASK_SERVER:
                        buffer.WriteString("minecraft:ask_server");
                        break;
                    case SuggestionTypes.ALL_RECIPES:
                        buffer.WriteString("minecraft:all_recipes");
                        break;
                    case SuggestionTypes.AVAILABLE_SOUNDS:
                        buffer.WriteString("minecraft:available_sounds");
                        break;
                    case SuggestionTypes.SUMMONABLE_ENTITIES:
                        buffer.WriteString("minecraft:summonable_entities");
                        break;
                    case SuggestionTypes.AVAILABLE_BIOMES:
                        buffer.WriteString("minecraft:available_biomes");
                        break;
                }
            }
        }

        public static CommandArgumentNode Create(string name, bool executable, string parser, SuggestionTypes suggestionTypes, List<object> data)
        {
            return new CommandArgumentNode((byte) 0x02.SetBitMask(0x10), executable, name, parser, suggestionTypes, data);
        }
    }

    public List<CommandNode> Nodes { get; set; } = new List<CommandNode>();
    public int RootIndex { get; set; }
    public void Read(InByteBuffer buffer)
    {
        try
        {
            int count = buffer.ReadVarInt();
            var nodes = buffer.ReadArray(ReadNode, count);
            Nodes.AddRange(nodes);
            RootIndex = buffer.ReadVarInt();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
    private CommandNode ReadNode(InByteBuffer buffer)
    {
        var flags = buffer.ReadByte();
        var nodeType = (NodeType) (flags & 0x3);
        return nodeType switch
        {
            NodeType.Root => new CommandRootNode(flags, buffer),
            NodeType.Literal => new CommandLiteralNode(flags, buffer),
            NodeType.Argument => new CommandArgumentNode(flags, buffer),
            _ => throw new Exception("Unknown node type"),
        };
    }

    public void Write(OutByteBuffer buffer)
    {
        buffer.WriteVarInt(Nodes.Count);
        for(int i = 0; i < Nodes.Count; i++)
        {
            var node = Nodes[i];
            node.Write(buffer);
        }
        buffer.WriteVarInt(RootIndex);
    }
    
    public async Task Handle(PacketHandleContext handler)
    {
        var proxyCommands = handler.Proxy.Plugins.RegisterCommands(handler.Player);
        var root = Nodes[RootIndex];


        foreach (var command in proxyCommands)
        {
            Nodes.Add(command.Value.Key);
            int index = Nodes.Count - 1;
            
            root.ChildrenIds.Add(index);

            if (command.Value.Value is not null)
            {
                foreach (var argument in command.Value.Value)
                {
                    Nodes.Add(argument);
                    int argumentIndex = Nodes.Count - 1;
                    if (!command.Value.Key.ChildrenIds.Any())
                    {
                        command.Value.Key.ChildrenIds.Add(argumentIndex);
                    }else
                    {
                        var childrenNode = Nodes[command.Value.Key.ChildrenIds[^1]];
                        childrenNode.ChildrenIds.Add(argumentIndex);
                    }
                }
            }
        }

        await handler.Player.SendMessageAsync($"Command tree received, {Nodes.Count} nodes, root index {RootIndex}");
    }
}

