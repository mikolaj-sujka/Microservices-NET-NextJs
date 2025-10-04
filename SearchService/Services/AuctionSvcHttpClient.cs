using MongoDB.Entities;
using SearchService.Models;

namespace SearchService.Services;

public class AuctionSvcHttpClient(HttpClient client, IConfiguration configuration)
{
    public async Task<List<Item>> GetItemsForSearchDb()
    {
        var lastUpdated = await DB.Find<Item, string>()
            .Sort(x => x.Descending(a => a.UpdatedAt))
            .Project(x => x.UpdatedAt.ToString())
            .ExecuteFirstAsync();

        return await client.GetFromJsonAsync<List<Item>>(
            configuration["AuctionServiceUrl"] + "/api/auctions?date=" + lastUpdated) ?? new List<Item>();
    }
}