using Microsoft.Extensions.Options;
using NewsPortal.Backend.Infrastructure.Http.HackerNews.Configuration;
using NewsPortal.Backend.Infrastructure.Http.HackerNews.Models;
using Newtonsoft.Json;
using RestSharp;

namespace NewsPortal.Backend.Infrastructure.Http.HackerNews.Client;

internal class HackerNewsClient : IHackerNewsClient
{
    private readonly RestClient _client;

    public HackerNewsClient(IOptions<HackerNewsOptions> options)
    {
        _client = new RestClient($"{options.Value.BaseUrl}/v{options.Value.Version}");
    }
    
    public async Task<Item> GetItemById(int itemId)
    {
        var itemUri = string.Format(Endpoints.Items.GetById, itemId);
        var req = new RestRequest(itemUri);
        req.AddQueryParameter("print", "pretty");

        var response = await _client.GetAsync(req);
        var item = JsonConvert.DeserializeObject<Item>(response.Content!);

        if (item is null) throw new Exception("Unexpected error in request processing");

        return item;
    }

    public async Task<List<int>> GetTopStories()
    {
        var req = new RestRequest(Endpoints.Items.TopStories);
        req.AddQueryParameter("print", "pretty");

        var response = await _client.GetAsync(req);
        var topStories = JsonConvert.DeserializeObject<List<int>>(response.Content!);

        if (topStories is null) throw new Exception("Unexpected error in request processing");

        return topStories;
    }

    public async Task<List<int>> GetNewStories()
    {
        var req = new RestRequest(Endpoints.Items.NewStories);
        req.AddQueryParameter("print", "pretty");

        var response = await _client.GetAsync(req);
        var newStories = JsonConvert.DeserializeObject<List<int>>(response.Content!);

        if (newStories is null) throw new Exception("Unexpected error in request processing");

        return newStories;
    }
}