using System.Text;
using System.Text.Json;
using BlazorMinesweeper.Components.Pages;
using BlazorMinesweeper.Configurations;
using BlazorMinesweeper.Dtos;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;



namespace BlazorMinesweeper.Clients;

public class GamesClient
{
    private readonly HttpClient _httpClient;

    private readonly string EndpointGame = "/Game"; //  Endpoint na vytvoření nové hry
    private readonly string EndpointActive = "/Game/active"; // Endpoint na vylistování všech dostupných her
    private readonly string EndpointField = "/GameField/"; //Endpoint na získání herního pole
    private readonly string EndpointClick = "/GameField/{0}/click"; //Endpoint na získání políček pro danou hru
    private readonly JsonSerializerOptions _options = new(JsonSerializerDefaults.Web);

    public GamesClient(HttpClient httpClient, IOptions<ServiceUrlConfig> config )
    { _httpClient = httpClient; 
      _httpClient.BaseAddress = new Uri(config.Value.GamesService);

        var authenticationString = "games-app:Karel*";
        var base64EncodedAuthenticationString = Convert.ToBase64String(
            Encoding.UTF8.GetBytes(authenticationString) );
        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Basic", base64EncodedAuthenticationString);
    }

    public async Task<GameDto?> Find(int id)
    {
        var a = await _httpClient.GetAsync(string.Format("{0}/{1}", EndpointGame, id));

        var aa = await a.Content.ReadAsStringAsync();
        if (!a.IsSuccessStatusCode)
            return null;

        return JsonSerializer.Deserialize<GameDto>(aa, _options);
    }

    public async Task<GameDto[]?> FindAll()
    {
        return await _httpClient.GetFromJsonAsync<GameDto[]?>(EndpointActive);
    }

    public async Task<int?> Create(GameInputDto game)
    {
        var result = await _httpClient.PostAsJsonAsync(EndpointGame, game);
        if(!result.IsSuccessStatusCode)
            throw new Exception(await result.Content.ReadAsStringAsync());

        var location = result.Headers.Location;
        if (location is null)
            return null;
        var lastSlash = location.OriginalString.LastIndexOf('/');
        var success = int.TryParse(location?.OriginalString.Substring(lastSlash + 1), out var id);
        return success ? id : null;
    }
    public async Task<bool> ClickGame(int gameId, GameClickDto input)
    {
        // Sestavení finální URL vložením gameId do templatu
        string endpoint = string.Format(EndpointClick, gameId);

        // Volání API s dynamicky sestavenou URL
        var result = await _httpClient.PostAsJsonAsync(endpoint, input);
        return result.IsSuccessStatusCode;
    }

    public async Task<GameFieldDto[,]> GetBoard(int gameId)
    {
        var response = await _httpClient.GetAsync(string.Format("{0}/{1}", EndpointField, gameId));

        if (response.IsSuccessStatusCode)
        {
            var boardJson = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var boardList = JsonSerializer.Deserialize<List<GameFieldDto>>(boardJson, options);

            // Převod lineárního seznamu zpět na dvourozměrné pole
            var size = (int)Math.Sqrt(boardList.Count);
            var board = new GameFieldDto[size, size];
            for (int i = 0; i < boardList.Count; i++)
            {
                var row = i / size;
                var col = i % size;
                board[row, col] = boardList[i];
            }
            return board;
        }
        else
        {
            return null;
        }
    }

}
