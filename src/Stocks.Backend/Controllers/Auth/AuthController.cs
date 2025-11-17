using Microsoft.AspNetCore.Mvc;
using Stocks.Backend.Dtos.Auth;
using Stocks.Backend.Service.Interfaces;

namespace Stocks.Backend.Controllers.Auth;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }
    
    [HttpPost("signup")]
    public async Task<IActionResult> SignUp([FromBody] SignupRequest request)
    {
        var response = await _authService.SignUp(request);

        return StatusCode(response.Code, response);
    }

    [HttpPost("signin")]
    public async Task<IActionResult> SignIn([FromBody] SignInRequest request)
    {
        var response = await _authService.SignIn(request);
        return StatusCode(response.Code, response);
    }
}