using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace NewsPortal.Backend.Infrastructure.Http.HackerNews.Models.Contracts;

/// <summary>
///     Generic item object. Items could be: Stories, Comments, Jobs, Ask HNs and Polls.
/// </summary>
public class Item
{
    public int Id { get; set; }

    public bool? Deleted { get; set; }

    public string Type { get; set; } = string.Empty;

    public string By { get; set; } = string.Empty;

    [JsonConverter(typeof(UnixDateTimeConverter))]
    public DateTime Time { get; set; }

    public string Text { get; set; } = string.Empty;

    public bool? Dead { get; set; }

    public int? Parent { get; set; }

    public int? Poll { get; set; }

    public List<int>? Kids { get; set; }

    public string? Url { get; set; }

    public int? Score { get; set; }

    public string? Title { get; set; }

    public List<int>? Parts { get; set; }

    public int? Descendants { get; set; }
}