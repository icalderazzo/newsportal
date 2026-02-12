namespace NewsPortal.Backend.Domain.Models.Items;

public class Item : BaseModel
{
    public required string Title { get; set; }
    public string? Url { get; set; }
    public DateTime? Time { get; set; }

    public IEnumerable<UserItem>? UserItems { get; set; }
}