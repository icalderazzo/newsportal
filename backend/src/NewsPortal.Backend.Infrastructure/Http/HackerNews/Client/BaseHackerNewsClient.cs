using Microsoft.Extensions.Options;
using NewsPortal.Backend.Infrastructure.Http.HackerNews.DependencyInjection;
using NewsPortal.Backend.Infrastructure.Http.HackerNews.Models;
using Newtonsoft.Json;
using RestSharp;

namespace NewsPortal.Backend.Infrastructure.Http.HackerNews.Client;

internal abstract class BaseHackerNewsClient
{
    protected readonly RestClient Client;

    protected BaseHackerNewsClient(IOptions<HackerNewsOptions> options)
    {
        Client = new RestClient($"{options.Value.BaseUrl}/v{options.Value.Version}");;
    }

    /// <summary>
    ///     Default GET Request to HackerNewsApi.
    /// </summary>
    /// <param name="uri">The endpoint.</param>
    /// <typeparam name="T">Type of the response object.</typeparam>
    /// <returns></returns>
    protected async Task<HackerNewsClientResponse<T>> Get<T>(string uri)
    {
        var result = new HackerNewsClientResponse<T>();
        
        var req = new RestRequest(uri);
        req.AddQueryParameter("print", "pretty");

        var response = await Client.ExecuteAsync(req);
        result.Code = response.StatusCode;
        
        if (!response.IsSuccessful) return result;

        result.Success = true;
        result.Data = JsonConvert.DeserializeObject<T>(response.Content!);
        return result;
    }
}