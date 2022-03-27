namespace Moonpie.Utils.Mojang;

public class PlayerTexture
{
    public PlayerTexture(string url)
    {
        Url = url;
    }

    public string Url { get; private set; }
    
    public byte[]? Data { get; private set; }

    public async Task Fetch()
    {
        Directory.CreateDirectory("cache/textures");
        var sha256 = Url.Substring(Url.LastIndexOf('/') + 1);
        if (File.Exists("cache/textures/" + sha256))
        {
            Data = await File.ReadAllBytesAsync("cache/textures/" + sha256);
            return;
        }
        var client = new HttpClient();
        var response = await client.GetAsync(Url);
        if (response.IsSuccessStatusCode)
        {
            Data = await response.Content.ReadAsByteArrayAsync();
            await File.WriteAllBytesAsync("cache/textures/" + sha256, Data);
        }
    }
}