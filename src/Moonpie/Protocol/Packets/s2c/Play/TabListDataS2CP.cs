using System.Text;
using Moonpie.Protocol.Network;
using Moonpie.Protocol.Protocol;
using Moonpie.Utils.Protocol;

namespace Moonpie.Protocol.Packets.s2c.Play;

public class TabListDataS2CP : IS2CPacket
{
    public enum TabListItemActions
    {
        Add,
        UpdateGamemode,
        UpdateLatency,
        UpdateDisplayName,
        Remove
    }
    
    public class TabListItemData
    {
        public string? Name { get; set; }
        public int? Ping { get; set; }
        public Gamemodes? Gamemode { get; set; }
        public bool? HasDisplayName { get; set; }
        public ChatComponent? DisplayName { get; set; }
        public PlayerProperties? Properties { get; set; }
        public bool Remove = false;
        public Team? Team { get; set; }
        public bool RemoveFromTeam = false;
    }
    public Dictionary<JavaUUID, TabListItemData> Items { get; set; } = new ();
    public TabListItemActions Action { get; set; }
    public void Read(InByteBuffer buffer)
    {
        try
        {
            if (buffer.Version < ProtocolVersion.v14w19a)
            {
                var name = buffer.ReadString();
                int ping;
                if (buffer.Version < ProtocolVersion.v14w04a)
                {
                    ping = buffer.ReadUShort();
                }else
                {
                    ping = buffer.ReadVarInt();
                }
            
                var action = buffer.ReadBoolean() ? TabListItemActions.UpdateLatency : TabListItemActions.Remove;
                Action = action;
                var uuid = GuidUtils.NameUUIDFromBytes(Encoding.UTF8.GetBytes(name));
                var item = new TabListItemData()
                {
                    Name = name,
                    Ping = ping,
                    Remove = action == TabListItemActions.Remove
                };
                Items.Add(JavaUUID.FromString(uuid), item);
            }
            else
            {
                var action = (TabListItemActions) buffer.ReadVarInt();
                Action = action;
                var count = buffer.ReadVarInt();
                for (int i = 0; i < count; i++)
                {
                    var uuid = buffer.ReadUUID();
                    TabListItemData? data = null;
                    switch (action)
                    {
                        case TabListItemActions.Add:
                        {
                            var name = buffer.ReadString();
                            var properties = buffer.ReadPlayerProperties();
                            var gamemode = (Gamemodes) buffer.ReadVarInt();
                            var ping = buffer.ReadVarInt();
                            var hasDisplayName = buffer.ReadBoolean();
                            ChatComponent? displayName = null;
                            if (hasDisplayName)
                            {
                                displayName = buffer.ReadChatComponent();
                            }
                            data = new TabListItemData()
                            {
                                Name = name,
                                Properties = properties,
                                Gamemode = gamemode,
                                Ping = ping,
                                HasDisplayName = hasDisplayName,
                                DisplayName = displayName,
                            };
                            break;
                        }
                        case TabListItemActions.UpdateGamemode:
                        {
                            data = new TabListItemData()
                            {
                                Gamemode = (Gamemodes) buffer.ReadVarInt(),
                            };
                            break;
                        }
                        case TabListItemActions.UpdateLatency:
                        {
                            data = new TabListItemData()
                            {
                                Ping = buffer.ReadVarInt(),
                            };
                            break;
                        }
                        case TabListItemActions.UpdateDisplayName:
                        {
                            var hasDisplayName = buffer.ReadBoolean();
                            ChatComponent? displayName = null;
                            if (hasDisplayName)
                            {
                                displayName = buffer.ReadChatComponent();
                            }
                        
                            data = new TabListItemData()
                            {
                                HasDisplayName = hasDisplayName,
                                DisplayName = displayName,
                            };
                            break;
                        }
                        case TabListItemActions.Remove:
                        {
                            data = new TabListItemData()
                            {
                                Remove = true,
                            };
                            break;
                        }
                    }

                    if (data is not null)
                    {
                        Items.Add(uuid, data);
                    }
                }
            }
        }catch(Exception e)
        {
            Console.WriteLine(e);
        }
    }

    public void Write(OutByteBuffer buffer)
    {
        try
        {
            if (buffer.Version < ProtocolVersion.v14w19a)
            {
                var data = Items.First().Value;
                buffer.WriteString(data.Name ?? "");

                if (buffer.Version < ProtocolVersion.v14w04a)
                {
                    buffer.WriteUShort((ushort)(data.Ping ?? 0));
                }else
                {
                    buffer.WriteVarInt(data.Ping ?? 0);
                }
            
                buffer.WriteBoolean(!data.Remove);
            }
            else
            {
                buffer.WriteVarInt((int)Action);
                buffer.WriteVarInt(Items.Count);
            
                foreach (var (uuid, data) in Items)
                {
                    switch (Action)
                    {
                        case TabListItemActions.Add:
                        {
                            buffer.WriteString(data.Name ?? "");
                            buffer.WritePlayerProperties(data.Properties ?? new PlayerProperties());
                            buffer.WriteVarInt((int)(data.Gamemode ?? Gamemodes.Creative));
                            buffer.WriteVarInt(data.Ping ?? 0);
                            buffer.WriteBoolean(data.HasDisplayName ?? false);
                            if (data.HasDisplayName ?? false)
                            {
                                buffer.WriteChatComponent(data.DisplayName ?? new ChatComponent("error"));
                            }
                            break;
                        }
                        case TabListItemActions.UpdateGamemode:
                        {
                            buffer.WriteVarInt((int)(data.Gamemode ?? Gamemodes.Creative));
                            break;
                        }
                        case TabListItemActions.UpdateLatency:
                        {
                            buffer.WriteVarInt(data.Ping ?? 0);
                            break;
                        }
                        case TabListItemActions.UpdateDisplayName:
                        {
                            buffer.WriteBoolean(data.HasDisplayName ?? false);
                            if (data.HasDisplayName ?? false)
                            {
                                buffer.WriteChatComponent(data.DisplayName ?? new ChatComponent("error"));
                            }
                            break;
                        }
                        case TabListItemActions.Remove:
                        {
                            break;
                        }
                    }
                }
            }
        }catch(Exception e)
        {
            Console.WriteLine(e);
        }
    }

    public Task Handle(PacketHandleContext handler)
    {
        handler.Cancel();
        return Task.CompletedTask;
    }
}