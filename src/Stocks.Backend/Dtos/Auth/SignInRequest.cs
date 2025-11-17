using System.ComponentModel.DataAnnotations;

namespace Stocks.Backend.Dtos.Auth;

public record SignInRequest(
    [Required, EmailAddress]
    string Email,

    [Required, MinLength(6)]
    string Password);