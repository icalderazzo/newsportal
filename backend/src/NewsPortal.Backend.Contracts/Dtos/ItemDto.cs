namespace NewsPortal.Backend.Contracts.Dtos;

public class ItemDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Text { get; set; }
    public string? Url { get; set; }
    public DateTime Time { get; set; }
}