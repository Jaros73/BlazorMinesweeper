using System.Text.Json;
using BlazorMinesweeper.Configurations;
using BlazorMinesweeper.Dtos;
using Microsoft.Extensions.Options;


namespace BlazorMinesweeper.Clients;

public class GamesClient
{
    private readonly HttpClient _httpClient;
    private readonly string _baseEndpoint = "/Game"; // Základní endpoint pro hry
    private readonly JsonSerializerOptions _options = new(JsonSerializerDefaults.Web);

    public GamesClient(HttpClient httpClient, IOptions<ServiceUrlConfig> config )
    { _httpClient = httpClient; _httpClient.BaseAddress = new Uri(config.Value.GamesService); }

    public async Task<GameDto?> Find(int id)
    {
        var a = await _httpClient.GetAsync($"{_baseEndpoint}/{id}");
        var aa = await a.Content.ReadAsStringAsync();
        if (!a.IsSuccessStatusCode)
            return null;

        return JsonSerializer.Deserialize<GameDto>(aa, _options);
    }

    public async Task<GameDto[]?> FindAll()
    {
        var Endpoint = $"{_baseEndpoint}/active";
        return await _httpClient.GetFromJsonAsync<GameDto[]?>(Endpoint);
    }

    public async Task<int?> Create(GameInputDto game)
    {
        var result = await _httpClient.PostAsJsonAsync(_baseEndpoint, game);
        if(!result.IsSuccessStatusCode)
            throw new Exception(await result.Content.ReadAsStringAsync());

        var location = result.Headers.Location;
        if (location != null)
            return null;
        var lastSlash = location.OriginalString.LastIndexOf('/');
        var success = int.TryParse(location?.OriginalString.Substring(lastSlash + 1), out var id);
        return success ? id : null;
    }
}
