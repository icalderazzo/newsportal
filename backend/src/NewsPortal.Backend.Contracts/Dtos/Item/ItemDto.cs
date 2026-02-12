namespace NewsPortal.Backend.Contracts.Dtos.Item;

public class ItemDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string? Text { get; set; }
    public string? Url { get; set; }
    public DateTime Time { get; set; }
    public bool IsBookmarked { get; set; }
}