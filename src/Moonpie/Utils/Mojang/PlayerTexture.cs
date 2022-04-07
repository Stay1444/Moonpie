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

using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

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