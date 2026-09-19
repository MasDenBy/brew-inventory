namespace BrewInventory.Infrastructure.Brewfather.Settings;

public class BrewfatherSettings
{
    public string UserId { get; set; } = null!;
    public string ApiKey { get; set; } = null!;
    public Uri BaseUrl { get; set; } = null!;
}
