using NewsPortal.Backend.Contracts.Dtos.Item.Story;

namespace NewsPortal.Backend.UnitTests.Application.Item.MockData;

public static class StoriesData
{
    public static readonly List<StoryDto> Stories = new()
    {
        new StoryDto
        {
            Id = 1,
            Title = "Story1",
            Type = "story",
            Url = "https://story1.com"
        },
        new StoryDto
        {
            Id = 2,
            Title = "Story2",
            Type = "story",
            Url = "https://story2.com"
        },
        new StoryDto
        {
            Id = 3,
            Title = "Story3",
            Type = "story",
            Url = "https://story3.com"
        },
        new StoryDto
        {
            Id = 4,
            Title = "Story4",
            Type = "story"
        },
    };
}