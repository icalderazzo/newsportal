using Microsoft.Extensions.Options;
using NewsPortal.Backend.Infrastructure.Http.HackerNews.Configuration;
using NewsPortal.Backend.Infrastructure.Http.HackerNews.Models;
using NewsPortal.Backend.Infrastructure.Http.HackerNews.Models.Contracts;
using Newtonsoft.Json;
using RestSharp;

namespace NewsPortal.Backend.Infrastructure.Http.HackerNews.Client;

internal sealed class HackerNewsClient : BaseHackerNewsClient, IHackerNewsClient
{
    public HackerNewsClient(IOptions<HackerNewsOptions> options) : base(options)
    {
    }
    
    public async Task<HackerNewsClientResponse<Item>> GetItemById(int itemId)
    {
        var itemUri = string.Format(Endpoints.Items.GetById, itemId);
        return await Get<Item>(itemUri);
    }

    public async Task<HackerNewsClientResponse<List<int>>> GetTopStories()
    {
        return await Get<List<int>>(Endpoints.Items.TopStories);
    }

    public async Task<HackerNewsClientResponse<List<int>>> GetNewStories()
    {
        return await Get<List<int>>(Endpoints.Items.NewStories);
    }
}