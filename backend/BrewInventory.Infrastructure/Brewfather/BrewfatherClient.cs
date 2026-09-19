using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using BrewInventory.Infrastructure.Brewfather.Models;
using BrewInventory.Infrastructure.Brewfather.Settings;
using Microsoft.Extensions.Options;

namespace BrewInventory.Infrastructure.Brewfather;

public class BrewfatherClient : IBrewfatherClient
{
    private readonly HttpClient _httpClient;
    private const int PageLimit = 50;

    private readonly static JsonSerializerOptions jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
    };

    public BrewfatherClient(
        HttpClient httpClient,
        IOptions<BrewfatherSettings> settings)
    {
        ArgumentNullException.ThrowIfNull(httpClient);
        ArgumentNullException.ThrowIfNull(settings);

        _httpClient = httpClient;

        var authString = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{settings.Value.UserId}:{settings.Value.ApiKey}"));
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authString);
        _httpClient.BaseAddress = settings.Value.BaseUrl;
    }

    public async Task<ICollection<T>> GetAllItemsAsync<T>(string endpoint, string? include = null, CancellationToken cancellationToken = default)
    {
        var allItems = new List<T>();
        string? startAfter = null;
        bool hasMore = true;

        while (hasMore)
        {
            var url = $"{endpoint}?limit={PageLimit}";
            if (!string.IsNullOrEmpty(startAfter))
            {
                url += $"&start_after={startAfter}";
            }
            if (!string.IsNullOrEmpty(include))
            {
                url += $"&include={include}";
            }

            var response = await _httpClient.GetAsync(new Uri(url, UriKind.Relative), cancellationToken);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync(cancellationToken);

            var items = JsonSerializer.Deserialize<List<T>>(content, jsonOptions);

            if (items == null || items.Count == 0)
            {
                hasMore = false;
            }
            else
            {
                allItems.AddRange(items);

                if (items.Count == PageLimit)
                {
                    var lastItem = items[^1];
                    var idProperty = typeof(T).GetProperty("Id") ?? typeof(T).GetProperty("_id");
                    if (idProperty != null)
                    {
                        startAfter = idProperty.GetValue(lastItem)?.ToString();
                    }
                    else
                    {
                        hasMore = false;
                    }
                }
                else
                {
                    hasMore = false;
                }
            }
        }

        return allItems;
    }

    public async Task<string> CreateRecipeAsync(BrewfatherCreateRecipeRequest recipe, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PostAsJsonAsync("recipes", recipe, cancellationToken);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync(cancellationToken);
        var result = JsonSerializer.Deserialize<BrewfatherCreateRecipeResponse>(content, jsonOptions);

        if (result is null || string.IsNullOrWhiteSpace(result.Id))
        {
            throw new InvalidOperationException("Brewfather API did not return a recipe id.");
        }

        return result.Id;
    }
}
