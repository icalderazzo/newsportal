using System.ComponentModel.DataAnnotations;

namespace NewsPortal.Backend.Domain.Models;

public abstract class BaseModel
{
    [Key] public int Id { get; set; }
}