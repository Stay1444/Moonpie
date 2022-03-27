using System.IO.Compression;
using Ionic.Zlib;
using Serilog;
using CompressionLevel = Ionic.Zlib.CompressionLevel;
using CompressionMode = Ionic.Zlib.CompressionMode;

namespace Moonpie.Utils.Protocol;

public class ProtocolCompression
{
    public static byte[] Compress(byte[] data)
    {
        using (MemoryStream memoryStream = new MemoryStream())
        {
            Stream compressor = new ZlibStream(memoryStream, CompressionMode.Compress, CompressionLevel.Default);
            compressor.Write(data, 0, data.Length);
            compressor.Flush();
            memoryStream.Flush();
            compressor.Close();
            return memoryStream.ToArray();
        }
    }
    
    public static byte[] Decompress(byte[] data)
    {
        using (MemoryStream memoryStream = new MemoryStream(data))
        {
            Stream decompressor = new ZlibStream(memoryStream, CompressionMode.Decompress, CompressionLevel.Default);
            var buffer = new byte[1024];
            var result = new List<byte>();
            while (true)
            {
                int read = decompressor.Read(buffer, 0, buffer.Length);
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
}