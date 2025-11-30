using Microsoft.AspNetCore.Identity;

namespace Stocks.Backend.Models.Auth;

public class AppUser : IdentityUser
{
    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public decimal AvailableBalance {get; set;}

    public decimal ReservedBalance {get; set;}

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
