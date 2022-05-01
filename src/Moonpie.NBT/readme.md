# Moonpie NBT

Minecraft Java NBT Format Serializer and Deserializer built for Moonpie.

Does not work with Minecraft Bedrock (Bedrock is LE instead of BE)
## Features
- Serialize and Deserialize NBT data
- Support all Minecraft (Java) NBT data types
- NBT Compression (GZip)
## Example

```cs
var nbt = NBTSerializer.Deserialize(bytes);
``` 
```cs
var bytes = NBTSerializer.Serialize(nbt);
```

