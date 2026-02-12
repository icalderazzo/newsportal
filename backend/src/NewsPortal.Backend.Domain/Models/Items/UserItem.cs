namespace NewsPortal.Backend.Domain.Models.Items;

public class UserItem
{
    public required int UserId { get; set; }
    public required int ItemId { get; set; }
    public DateTime SavedAt { get; set; }

    public User User { get; set; }
    public Item Item { get; set; }
}