namespace NewsPortal.Backend.UnitTests.Application.Item.MockData;

public static class HackerNewsData
{
    public static readonly List<int> NewestStories = new() { 1, 2, 3, 4 };

    public static readonly List<Infrastructure.Http.HackerNews.Models.Contracts.Item> Items = new()
    {
        new Infrastructure.Http.HackerNews.Models.Contracts.Item
        {
            Id = 1,
            Title = "Story1",
            Type = "story",
            Url = "https://story1.com"
        },
        new Infrastructure.Http.HackerNews.Models.Contracts.Item
        {
            Id = 2,
            Title = "Story2",
            Type = "story",
            Url = "https://story2.com"
        },
        new Infrastructure.Http.HackerNews.Models.Contracts.Item
        {
            Id = 3,
            Title = "Story3",
            Type = "story",
            Url = "https://story3.com"
        },
        new Infrastructure.Http.HackerNews.Models.Contracts.Item
        {
            Id = 4,
            Title = "Story4",
            Type = "story"
        }
    };
}