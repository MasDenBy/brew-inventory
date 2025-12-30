using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using BrewInventory.App.Models;
using Microsoft.Extensions.Options;

namespace BrewInventory.App.Services;

public class BrewfatherClient : IBrewfatherClient
{
    private readonly HttpClient _httpClient;
    private readonly BrewfatherSettings _settings;
    private readonly ILogger<BrewfatherClient> _logger;
    private const int PageLimit = 50;

    public BrewfatherClient(
        HttpClient httpClient,
        IOptions<BrewfatherSettings> settings,
        ILogger<BrewfatherClient> logger)
    {
        _httpClient = httpClient;
        _settings = settings.Value;
        _logger = logger;

        // Set up authentication
        var authString = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{_settings.UserId}:{_settings.ApiKey}"));
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authString);
        _httpClient.BaseAddress = new Uri(_settings.BaseUrl);
    }

    public async Task<List<T>> GetAllInventoryItemsAsync<T>(string endpoint, CancellationToken cancellationToken = default)
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

            try
            {
                var response = await _httpClient.GetAsync(url, cancellationToken);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync(cancellationToken);
                var items = JsonSerializer.Deserialize<List<T>>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (items == null || items.Count == 0)
                {
                    hasMore = false;
                }
                else
                {
                    allItems.AddRange(items);

                    // If we got exactly PageLimit items, there might be more
                    if (items.Count == PageLimit)
                    {
                        // Get the _id of the last item for pagination
                        var lastItem = items[^1];
                        var idProperty = typeof(T).GetProperty("_id");
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching data from Brewfather API: {Endpoint}", url);
                throw;
            }
        }

        return allItems;
    }
}
