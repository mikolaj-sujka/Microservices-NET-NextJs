using MongoDB.Driver;
using MongoDB.Entities;
using SearchService.Models;
using SearchService.Services;

namespace SearchService.Data;

public class DbInitializer
{
    public static async Task InitializeAsync(WebApplication app)
    {
        await DB.InitAsync("SearchDb",
            MongoClientSettings.FromConnectionString(
                app.Configuration.GetConnectionString("MongoDbConnection")));

        await DB.Index<Item>()
            .Key(i => i.Make, KeyType.Text)
            .Key(i => i.Model, KeyType.Text)
            .Key(i => i.Color, KeyType.Text)
            .CreateAsync();

        var count = await DB.CountAsync<Item>();

        using var scope = app.Services.CreateScope();
        var auctionClient = scope.ServiceProvider.GetRequiredService<AuctionSvcHttpClient>();

        var items = await auctionClient.GetItemsForSearchDb();

        Console.WriteLine($"Seeding {items.Count} items into SearchDb return from AuctionService...");

        if ( items.Count > 0) await DB.SaveAsync(items);
    }
}