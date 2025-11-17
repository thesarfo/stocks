using System.ComponentModel.DataAnnotations;

namespace Stocks.Backend.Dtos.Auth;

public record SignupRequest(
    [Required]
    string FirstName,

    [Required]
    string LastName,

    [Required, EmailAddress]
    string Email,

    [Required, MinLength(6)]
    string Password
);
