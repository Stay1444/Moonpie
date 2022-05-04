using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Moonpie.Mojang.Exceptions;
using Moonpie.Mojang.Models;

namespace Moonpie.Mojang;

public class MojangApiClient
{
    private HttpClient _httpClient;
    public MojangApiClient()
    {
        _httpClient = new HttpClient();
        _httpClient.DefaultRequestHeaders.Add("User-Agent", "Moonpie/1.0");
    }
    
    /// <summary>
    ///  <para>This will return the UUID of the name at the timestamp provided.
    ///  Timestamp can be used to get the UUID of the original user of that username, however, it only works if the name was changed at least once, or if the account is legacy.</para>
    ///
    ///     <para>The timestamp is a UNIX timestamp (without milliseconds)
    ///     When the at parameter is not sent, the current time is used</para>
    /// <para/>
    ///     Response
    ///     <code>
    ///     {
    ///         "name": "KrisJelbring",<br/>
    ///         "id": "7125ba8b1c864508b92bb5c042ccfe2b"<br/>
    ///     }
    ///     </code>
    ///     <para/>
    /// id is the uuid<br/>
    /// name is the current name of that uuid, it is not the name requested!<br/>
    /// legacy only appears when true (not migrated to mojang account)<br/>
    /// demo only appears when true (account unpaid)<br/>
    /// <para/>
    /// If there is no player with the given username an HTTP status code 204 (No Content) is sent without any HTTP body.<br/>
    /// If the timestamp is not a number, too big or too small the HTTP status code 400 (Bad Request) is sent with an error message looking like this:<br/>
    /// <code>
    /// {
    ///     "error": "IllegalArgumentException",
    ///     "errorMessage": "Invalid timestamp."
    /// }
    /// </code>
    /// </summary>
    /// <param name="username">Player Username</param>
    /// <param name="timestamp">TimeStamp to check</param>
    /// <returns></returns>
    /// <exception cref="MojangBadRequestException">If the timestamp is not a number, too big or too small</exception>
    /// <exception cref="Exception">In case that an unknown code is returned</exception>
    public async Task<ProfileModel?> GetProfileAsync(string username, DateTime? timestamp = null)
    {
        var response = await _httpClient.GetAsync($"https://api.mojang.com/users/profiles/minecraft/{username}" + (timestamp != null ? $"?at={new DateTimeOffset(timestamp.Value).ToUnixTimeSeconds()}" : ""));
        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<ProfileModel>(json, new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            });
        }
         
        if (response.StatusCode == HttpStatusCode.BadRequest)
        {
            var error = JsonSerializer.Deserialize<ErrorModel>(await response.Content.ReadAsStringAsync(), new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            });

            throw new MojangBadRequestException(error ?? new ErrorModel("Unknown error", "Unknown error"));
        }
        
        throw new Exception("Unknown error. Status code: " + response.StatusCode);
    }

    public async Task<IEnumerable<ProfileModel>> GetProfilesAsync(params string[] names)
    {
        const int maxProfilesPerRequest = 10;
        
        if (names.Length == 0)
            return Array.Empty<ProfileModel>();
        
        if (names.Length > maxProfilesPerRequest)
            throw new ArgumentException("Too many names. Max is " + maxProfilesPerRequest);
        
        var response = await _httpClient.PostAsync("https://api.mojang.com/profiles/minecraft", new StringContent(JsonSerializer.Serialize(names), Encoding.UTF8, "application/json"));
        
        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<ProfileModel[]>(json, new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            }) ?? Array.Empty<ProfileModel>();
        }
        
        if (response.StatusCode == HttpStatusCode.BadRequest)
        {
            var error = JsonSerializer.Deserialize<ErrorModel>(await response.Content.ReadAsStringAsync(), new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            });

            throw new MojangBadRequestException(error ?? new ErrorModel("Unknown error", "Unknown error"));
        }
        
        throw new Exception("Unknown error. Status code: " + response.StatusCode);
    }
    
    public async Task<IEnumerable<NameHistoryModel>?> GetNameHistoryAsync(Guid id)
    {
        var response = await _httpClient.GetAsync($"https://api.mojang.com/user/profiles/{id}/names");
        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<NameHistoryModel[]>(json, new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            });
        }
        
        if (response.StatusCode == HttpStatusCode.BadRequest)
        {
            var error = JsonSerializer.Deserialize<ErrorModel>(await response.Content.ReadAsStringAsync(), new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            });

            throw new MojangBadRequestException(error ?? new ErrorModel("Unknown error", "Unknown error"));
        }
        
        if (response.StatusCode == HttpStatusCode.NotFound) return null;
        
        throw new Exception("Unknown error. Status code: " + response.StatusCode);
    }

    public async Task<FullProfileModel?> GetFullProfile(Guid id, bool unsigned = true)
    {
        var response = await _httpClient.GetAsync($"https://sessionserver.mojang.com/session/minecraft/profile/{id}?unsigned={unsigned}");
        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<FullProfileModel>(json, new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            });
        }
        
        if (response.StatusCode == HttpStatusCode.BadRequest)
        {
            var error = JsonSerializer.Deserialize<ErrorModel>(await response.Content.ReadAsStringAsync(), new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            });

            throw new MojangBadRequestException(error ?? new ErrorModel("Unknown error", "Unknown error"));
        }
        
        if (response.StatusCode == HttpStatusCode.NotFound) return null;

        return null;
    }

    public async Task<string[]> GetBlockedServers()
    {
        var response = await _httpClient.GetAsync("https://sessionserver.mojang.com/blockedservers");
        if (response.IsSuccessStatusCode)
        {
            var result = new List<string>();
            var content = await response.Content.ReadAsStringAsync();
            foreach (var line in content.Split("\n"))
            {
                result.Add(line.Trim());
            }
            
            return result.ToArray();
        }
        
        if (response.StatusCode == HttpStatusCode.BadRequest)
        {
            var error = JsonSerializer.Deserialize<ErrorModel>(await response.Content.ReadAsStringAsync(), new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            });

            throw new MojangBadRequestException(error ?? new ErrorModel("Unknown error", "Unknown error"));
        }



        return new string[] { };
    }

    public async Task<bool> IsServerBlocked(string address)
    {
        // We hash the address with SHA-1 and then check it against GetBlockedServers
        
        var sha1 = SHA1.Create();
        var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(address));
        var hex = BitConverter.ToString(hash).Replace("-", "").ToLower();
        var result = await GetBlockedServers();
        return result.Contains(hex);
    }
}