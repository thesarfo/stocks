namespace Stocks.Backend.Dtos.Auth;

public record UserResponse(
    string FirstName,
    string LastName,
    string Email,
    string Token
    )
{
}