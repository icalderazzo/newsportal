using NewsPortal.Backend.Domain.Models.Items;

namespace NewsPortal.Backend.UnitTests.Application.Item.MockData;

public static class UserItemsData
{
    public static readonly List<UserItem> BookmarkedItems = new()
    {
        new UserItem
        {
            UserId = 1,
            ItemId = 1,
            SavedAt = DateTime.UtcNow,
            Item = new Story
            {
                Id = 1,
                Title = "Story1",
                Url = "https://story1.com"
            }
        },
        new UserItem
        {
            UserId = 1,
            ItemId = 2,
            SavedAt = DateTime.UtcNow.AddDays(-1),
            Item = new Story
            {
                Id = 2,
                Title = "Story2",
                Url = "https://story2.com"
            }
        },
        new UserItem
        {
            UserId = 1,
            ItemId = 3,
            SavedAt = DateTime.UtcNow.AddDays(-2),
            Item = new Story
            {
                Id = 3,
                Title = "Story3",
                Url = "https://story3.com"
            }
        }
    };
}


