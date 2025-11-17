using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Stocks.Backend.Dtos;
using Stocks.Backend.Dtos.Auth;
using Stocks.Backend.Models.Auth;
using Stocks.Backend.Options.Auth;
using Stocks.Backend.Repository.Interfaces;
using Stocks.Backend.Service.Interfaces;

namespace Stocks.Backend.Service.Providers;

public class AuthService : IAuthService
{
    private readonly ILogger<AuthService> _logger;
    private readonly IUserRepository _userRepository;
    private readonly JwtConfig _jwtConfig;

    public AuthService(ILogger<AuthService> logger, IUserRepository userRepository, IOptions<JwtConfig> jwtConfig)
    {
        _logger = logger;
        _userRepository = userRepository;
        _jwtConfig = jwtConfig.Value;
    }

    public async Task<ApiResponse<UserResponse>> SignUp(SignupRequest request)
    {
        try
        {
            var existingUser = await _userRepository.GetUserByEmailAsync(request.Email);
            if (existingUser != null)
            {
                _logger.LogWarning("Attempted to create user with existing email: {Email}", request.Email);
                var errors = new[] { new ErrorResponse("Email", "User with this email already exists") };
                return new ApiResponse<UserResponse>("User already exists", (int)HttpStatusCode.BadRequest, null, "0", errors);
            }

            // Create user entity
            var user = new AppUser
            {
                UserName = request.Email,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            // Create user
            var createResult = await _userRepository.CreateUserAsync(user, request.Password);
            if (!createResult.Succeeded)
            {
                var errors = createResult.Errors.Select(e => new ErrorResponse("User", e.Description));
                _logger.LogError("Failed to create user {Email}: {Errors}", request.Email, string.Join(", ", createResult.Errors.Select(e => e.Description)));
                return new ApiResponse<UserResponse>("Failed to create user", (int)HttpStatusCode.BadRequest, null, "0", errors);
            }

            // Ensure default role exists
            const string defaultRole = "BASIC";
            if (!await _userRepository.RoleExistsAsync(defaultRole))
            {
                var roleResult = await _userRepository.CreateRoleAsync(defaultRole);
                if (!roleResult.Succeeded)
                {
                    var errors = roleResult.Errors.Select(e => new ErrorResponse("Role", e.Description));
                    _logger.LogError("Failed to create role {Role}: {Errors}", defaultRole, string.Join(", ", roleResult.Errors.Select(e => e.Description)));
                    return new ApiResponse<UserResponse>("Failed to create default role", (int)HttpStatusCode.InternalServerError, null, "0", errors);
                }
                _logger.LogInformation("Created role: {Role}", defaultRole);
            }

            // Assign user to default role
            var addToRoleResult = await _userRepository.AddToRoleAsync(user, defaultRole);
            if (!addToRoleResult.Succeeded)
            {
                var errors = addToRoleResult.Errors.Select(e => new ErrorResponse("Role", e.Description));
                _logger.LogError("Failed to assign role {Role} to user {Email}: {Errors}", 
                    defaultRole, request.Email, string.Join(", ", addToRoleResult.Errors.Select(e => e.Description)));
                
                // User was created but role assignment failed - this is a warning, not a failure
                _logger.LogWarning("User {Email} created but role assignment failed", request.Email);
            }

            _logger.LogInformation("Successfully created user: {Email}", request.Email);
            var roles = await _userRepository.GetUserRolesAsync(user);
            
            var token = await GenerateJwtToken(user, roles);
            
            var userResponse = new UserResponse(
                user.FirstName,
                user.LastName,
                user.Email,
                token
            );


            return new ApiResponse<UserResponse>("User registered successfully", (int)HttpStatusCode.Created, userResponse);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error during user signup for email: {Email}", request?.Email);
            return new ApiResponse<UserResponse>("An unexpected error occurred during signup", (int)HttpStatusCode.InternalServerError);
        }
    }

    public async Task<ApiResponse<UserResponse>> SignIn(SignInRequest request)
    {
        var user = await _userRepository.GetUserByEmailAsync(request.Email);
        if (user == null)
        {
            _logger.LogInformation("User with email {Email} does not exist", request.Email);
            return new ApiResponse<UserResponse>(
                "User with this email does not exist",
                StatusCodes.Status400BadRequest,
                null
            );
        }
        
        var isPasswordValid = await _userRepository.CheckPasswordAsync(user, request.Password);
        if (!isPasswordValid)
        {
            _logger.LogInformation("Invalid password for user with email {Email}", request.Email);
            return new ApiResponse<UserResponse>(
                "Invalid password",
                StatusCodes.Status400BadRequest,
                null
            );
        }

        var roles = await _userRepository.GetUserRolesAsync(user);
        var token = await GenerateJwtToken(user, roles);
        
        var userResponse = new UserResponse(
            user.FirstName,
            user.LastName,
            user.Email,
            token
        );
        
        _logger.LogInformation("User {Email} signed in successfully", request.Email);
        return new ApiResponse<UserResponse>(
            "User signed in successfully",
            StatusCodes.Status200OK,
            userResponse
        );
    }

    public async Task<ApiResponse<UserResponse>> VerifyEmail(string email)
    {
        // 1. send verification email
        // 2. when link is clicked, verify the token
        // 3. update user email verified status
        return new ApiResponse<UserResponse>(
            "Email verification not implemented",
            StatusCodes.Status501NotImplemented,
            null
        );
    }

    private Task<string> GenerateJwtToken(IdentityUser user, IList<string> roles)
    {
        var jwtTokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_jwtConfig.Secret);

        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim("Id", user.Id),
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                new Claim("Roles", string.Join(",", roles))
            }),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
        };

        var token = jwtTokenHandler.CreateToken(tokenDescriptor);
        var jwtToken = jwtTokenHandler.WriteToken(token);

        return Task.FromResult(jwtToken);
    }
}