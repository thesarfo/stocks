using Stocks.Backend.Dtos;
using Stocks.Backend.Dtos.Auth;

namespace Stocks.Backend.Service.Interfaces;

public interface IAuthService
{
    Task<ApiResponse<UserResponse>> SignUp(SignupRequest request);
    Task<ApiResponse<UserResponse>> SignIn(SignInRequest request);
    Task<ApiResponse<UserResponse>> VerifyEmail(string email);
}