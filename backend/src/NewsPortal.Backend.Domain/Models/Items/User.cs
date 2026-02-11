namespace NewsPortal.Backend.Domain.Models.Items;

public class User : BaseModel
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }

    public IEnumerable<UserItem>? UserItems { get; set; }
}