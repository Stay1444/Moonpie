using System.Reflection;
using System.Text.Json;
using Moonpie.Protocol.Network;
using Moonpie.Protocol.Packets;
using Moonpie.Protocol.Packets.c2s;
using Moonpie.Protocol.Packets.c2s.Handshaking;
using Moonpie.Protocol.Packets.c2s.Login;
using Moonpie.Protocol.Packets.c2s.Status;
using Moonpie.Protocol.Packets.s2c;
using Moonpie.Protocol.Packets.s2c.Login;
using Moonpie.Protocol.Packets.s2c.Status;
using Moonpie.Protocol.Protocol.Mapping;
using Serilog;

namespace Moonpie.Protocol.Protocol;

public class PacketMapper
{
    
    private const bool LOG = false;
    public PacketMapper()
    {
        MapIfNecessary();
    }

    public int? GetPacketId(PacketTypes.C2S packet, ProtocolVersion ProtocolVersion)
    {
        if (LOG) Log.Debug("GetPacketId: {0}", packet.ToString());
        // check if static mappings contains that packet. Static mappings usually contain packets from STATUS and LOGIN since this packets do not change.
        if (_c2sStaticMappings.ContainsKey(packet))
        {
            return _c2sStaticMappings[packet].Value.Value;
        }
        
        var vp = _mappings![ProtocolVersion];

        if (vp.MapVersion is not null) vp = _mappings[vp.MapVersion];

        return vp.C2S!.IndexOf(packet);
    }

    public int? GetPacketId(PacketTypes.S2C packet, ProtocolVersion ProtocolVersion)
    {
        if (LOG) Log.Debug("GetPacketId: {0}", packet.ToString());
        // check if static mappings contains that packet. Static mappings usually contain packets from STATUS and LOGIN since this packets do not change.
        if (_s2cStaticMappings.ContainsKey(packet))
        {
            return _s2cStaticMappings[packet].Value.Value;
        }
        
        var vp = _mappings![ProtocolVersion];

        if (vp.MapVersion is not null) vp = _mappings[vp.MapVersion];

        return vp.S2C!.IndexOf(packet);
    }

    public PacketTypes.C2S? GetC2SPacket(int id, ProtocolVersion ProtocolVersion, ProtocolState state)
    {

        if (state != ProtocolState.Play)
        {
            return _c2sStaticMappings.FirstOrDefault(x => x.Value.Value.Value == id && x.Value.Key == state).Key;
        }
        
        var vp = _mappings![ProtocolVersion];

        if (vp.MapVersion is not null) vp = _mappings[vp.MapVersion];

        return vp.C2S!.ElementAtOrDefault(id);
    }

    public PacketTypes.S2C? GetS2CPacket(int id, ProtocolVersion ProtocolVersion, ProtocolState state)
    {
        if (state != ProtocolState.Play)
        {
            return _s2cStaticMappings.FirstOrDefault(x => x.Value.Value.Value == id && x.Value.Key == state ).Key;
        }
        
        var vp = _mappings![ProtocolVersion];

        if (vp.MapVersion is not null) vp = _mappings[vp.MapVersion];

        return vp.S2C!.ElementAtOrDefault(id);
    }

    public PacketTypes.S2C? GetS2CPacket(IS2CPacket packet)
    {
        if (packet is GenericS2CP generic)
        {
            return generic.Type;
        }
        
        if (_s2cStaticMappings.Any(x => x.Value.Value.Key == packet.GetType()))
        {
            return _s2cStaticMappings.First(x => x.Value.Value.Key == packet.GetType()).Key;
        }
        
        foreach (var cmap in _s2cMappings!)
            if (cmap.Value == packet.GetType())
                return cmap.Key;

        return null;
    }

    public PacketTypes.C2S? GetC2SPacket(IC2SPacket packet)
    {
        
        if (packet is GenericC2SP generic)
        {
            return generic.Type;
        }
        
        if (_c2sStaticMappings.Any(x => x.Value.Value.Key == packet.GetType()))
        {
            return _c2sStaticMappings.First(x => x.Value.Value.Key == packet.GetType()).Key;
        }
        
        foreach (var cmap in _c2sMappings!)
            if (cmap.Value == packet.GetType())
                return cmap.Key;

        return null;
    }

    public Type GetPacketType(PacketTypes.C2S packet)
    {
        if (_c2sStaticMappings.ContainsKey(packet))
        {
            return _c2sStaticMappings[packet].Value.Key;
        }

        if (_c2sMappings!.ContainsKey(packet))
        {
            return _c2sMappings[packet];
        }

        return typeof(GenericC2SP);
    }

    public Type GetPacketType(PacketTypes.S2C packet)
    {
        if (_s2cStaticMappings.ContainsKey(packet))
        {
            return _s2cStaticMappings[packet].Value.Key;
        }
        
        if (_s2cMappings!.ContainsKey(packet))
        {
            return _s2cMappings[packet];
        }
        
        return typeof(GenericS2CP);
    }
    
    public IC2SPacket DeserializeC2SPacket(InByteBuffer buffer)
    {
        var ProtocolVersion = buffer.Version;
        int id = buffer.ReadVarInt();
        var packetType = GetC2SPacket(id, ProtocolVersion, buffer.State);

        if (packetType is null) throw new Exception($"Could not find C2S packet with id {id}");
 
        var packetInstanceType = GetPacketType(packetType.Value);
        IC2SPacket? packet;

        if (packetInstanceType == typeof(IPacketProvider))
        {
            var provider = (IPacketProvider?) Activator.CreateInstance(packetInstanceType);

            packet = (IC2SPacket) provider?.GetPacket(buffer)!;
            buffer.Reset();
        }
        else if (packetInstanceType == typeof(GenericC2SP))
        {
            packet = new GenericC2SP(packetType.Value);
        }else
        {
            packet = (IC2SPacket?) Activator.CreateInstance(packetInstanceType);
        }

        if (packet is null) throw new Exception($"Could not create C2S packet with id {id}");

        packet.Read(buffer);
        if (LOG)
        {
            if (packet is GenericC2SP generic)
            {
                Log.Debug("Deserialized C2S packet: {0}", generic.Type);
            }
            else
            {
                Log.Debug("Deserialized C2S packet: {0}", packet.GetType().Name);
            }
        }
        return packet;
    }

    public IS2CPacket DeserializeS2CPacket(InByteBuffer buffer)
    {
        var ProtocolVersion = buffer.Version;
        int id = buffer.ReadVarInt();
        var packetType = GetS2CPacket(id, ProtocolVersion,buffer.State);

        if (packetType is null) throw new Exception($"Could not find S2C packet with id {id}");

        IS2CPacket? packet;

        var packetInstanceType = GetPacketType(packetType.Value);
        if (packetInstanceType == typeof(IPacketProvider))
        {
            var provider = (IPacketProvider?) Activator.CreateInstance(packetInstanceType);

            packet = (IS2CPacket)provider?.GetPacket(buffer)!;
        }
        else if (packetInstanceType == typeof(GenericS2CP))
        {
            packet = new GenericS2CP(packetType.Value);
        }else
        {
            packet = (IS2CPacket?) Activator.CreateInstance(packetInstanceType);
        }

        if (packet is null) throw new Exception($"Could not create S2C packet with id {id}");

        packet.Read(buffer);

        if (LOG)
        {
            if (packet is GenericS2CP generic)
            {
                Log.Debug("Deserialized S2C packet: {0}", generic.Type);
            }
            else
            {
                Log.Debug("Deserialized S2C packet: {0}", packet.GetType().Name);
            }
        }
        return packet;
    }
    
    #region Static Mapping Region

    private static Dictionary<ProtocolVersion, VersionMapping>? _mappings;
    private static Dictionary<PacketTypes.C2S, Type>? _c2sMappings;
    private static Dictionary<PacketTypes.S2C, Type>? _s2cMappings;

    private static Dictionary<PacketTypes.C2S, KeyValuePair<ProtocolState,KeyValuePair<Type, int>>> _c2sStaticMappings = new()
    {
        {
            PacketTypes.C2S.HANDSHAKING_HANDSHAKE,
            new ( ProtocolState.Handshaking,
            new (typeof(HandshakeC2SP), 0))
        },
        {
            PacketTypes.C2S.STATUS_REQUEST,
            new ( ProtocolState.Status,
            new (typeof(StatusRequestC2SP), 0))
        },
        {
            PacketTypes.C2S.STATUS_PING,
            new ( ProtocolState.Status,
            new (typeof(StatusPingC2SP), 1))
        },
        {
            PacketTypes.C2S.LOGIN_LOGIN_START,
            new ( ProtocolState.Login,
            new (typeof(LoginStartC2SP), 0))
        },
        {
            PacketTypes.C2S.LOGIN_ENCRYPTION_RESPONSE,
            new ( ProtocolState.Login,
            new (typeof(EncryptionResponseC2SP), 1))
        },
        {
            PacketTypes.C2S.LOGIN_PLUGIN_RESPONSE,
            new ( ProtocolState.Login,
            new (typeof(LoginPluginResponseC2SP), 2))
        }
    };
    
    private static Dictionary<PacketTypes.S2C, KeyValuePair<ProtocolState,KeyValuePair<Type, int>>> _s2cStaticMappings = new()
    {
        {
            PacketTypes.S2C.LOGIN_KICK,
            new ( ProtocolState.Login,
            new (typeof(LoginKickS2CP), 0))
        },
        {
            PacketTypes.S2C.LOGIN_ENCRYPTION_REQUEST,
            new ( ProtocolState.Login,
            new (typeof(EncryptionRequestS2CP), 1))
        },
        {
            PacketTypes.S2C.LOGIN_LOGIN_SUCCESS,
            new ( ProtocolState.Login,
            new (typeof(LoginSuccessS2CP), 2))
        },
        {
            PacketTypes.S2C.LOGIN_COMPRESSION_SET,
            new ( ProtocolState.Login,
            new (typeof(CompressionSetS2CP), 3))
        },
        {
            PacketTypes.S2C.LOGIN_PLUGIN_REQUEST,
            new ( ProtocolState.Login,
            new (typeof(LoginPluginRequestS2CP), 4))
        },
        {
            PacketTypes.S2C.STATUS_RESPONSE,
            new ( ProtocolState.Status,
            new (typeof(ServerStatusResponseS2CP), 0))
        },
        {
            PacketTypes.S2C.STATUS_PONG,
            new ( ProtocolState.Status,
            new (typeof(StatusPongS2CP), 1))
        }
    };
    
    private void MapIfNecessary()
    {
        if (_mappings is null)
        {
            
            var assembly = Assembly.GetAssembly(typeof(PacketMapper));
            if (assembly == null)
                throw new Exception("Could not find assembly");

            var mappingsJsonResource =
                assembly.GetManifestResourceStream("Moonpie.Resources.version_mappings.json");

            if (mappingsJsonResource == null)
                throw new Exception("Could not find mappings resource");

            var mappingsJson = new StreamReader(mappingsJsonResource).ReadToEnd();

            var rawMap = JsonSerializer.Deserialize<Dictionary<int, RawMapModel>>(mappingsJson);

            if (rawMap == null)
                throw new Exception("Could not deserialize mappings");

            _mappings = new Dictionary<ProtocolVersion, VersionMapping>();

            
            foreach (var v in rawMap)
            {
                var mapping = new VersionMapping();
                if (!ProtocolVersion.TryFromName(v.Value.Name, out var version))
                    throw new Exception($"Could not find version {v.Value.Name}");
                mapping.Version = version;
                if (v.Value.Mapping is null)
                    throw new Exception("Mapping is null");
                var mappingJson = (JsonElement) v.Value.Mapping;
                if (mappingJson.ValueKind == JsonValueKind.Number)
                {
                    if (ProtocolVersion.TryFromValue(mappingJson.GetInt32(), out var mVersion))
                    {
                        mapping.MapVersion = mVersion;
                    }
                    else if (ProtocolVersion.TryFromValue(
                                 rawMap.First(x => x.Key == mappingJson.GetInt32()).Value.ProtocolId ?? 0,
                                 out mVersion))
                    {
                        mapping.MapVersion = mVersion;
                    }
                    else
                        throw new Exception("Could not find mapping version");
                }
                else
                {
                    var rawModel = mappingJson.Deserialize<RawMapVersionModel>();

                    if (rawModel is null)
                    {
                        Console.WriteLine("Could not deserialize mapping");
                        continue;
                    }

                    if (rawModel.C2S is not null)
                        foreach (var c2sEntry in rawModel.C2S)
                        {
                            if (!Enum.TryParse(c2sEntry, out PacketTypes.C2S c2sResult))
                                throw new Exception($"Could not parse C2S packet {c2sEntry}");

                            mapping.C2S!.Add(c2sResult);
                        }

                    if (rawModel.S2C is not null)
                        foreach (var s2cEntry in rawModel.S2C)
                        {
                            if (!Enum.TryParse(s2cEntry, out PacketTypes.S2C s2cResult))
                                throw new Exception($"Could not parse S2C packet {s2cEntry}");

                            mapping.S2C!.Add(s2cResult);
                        }
                }

                _mappings.Add(mapping.Version, mapping);
            }
        }

        if (_c2sMappings is null || _s2cMappings is null)
        {
            _c2sMappings = new Dictionary<PacketTypes.C2S, Type>();
            _s2cMappings = new Dictionary<PacketTypes.S2C, Type>();
            
            var assembly = Assembly.GetAssembly(typeof(PacketMapper));
            if (assembly == null)
                throw new Exception("Could not find assembly");

            var maps = assembly.GetTypes().Where(t => t.GetCustomAttributes(typeof(PacketType), false).Any());

            foreach (var map in maps)
            {
                var attr = (PacketType) map.GetCustomAttributes(typeof(PacketType), false).First();

                if (attr.C2S is not null) _c2sMappings.Add(attr.C2S.Value, map);

                if (attr.S2C is not null) _s2cMappings.Add(attr.S2C.Value, map);
            }
        }
    }

    #endregion
}