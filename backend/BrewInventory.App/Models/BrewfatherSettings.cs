namespace BrewInventory.App.Models;

public class BrewfatherSettings
{
    public string UserId { get; set; } = string.Empty;
    public string ApiKey { get; set; } = string.Empty;
    public string BaseUrl { get; set; } = "https://api.brewfather.app/v2";
}
