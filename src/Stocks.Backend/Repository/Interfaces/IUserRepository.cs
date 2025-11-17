using Microsoft.AspNetCore.Identity;
using Stocks.Backend.Models.Auth;

namespace Stocks.Backend.Repository.Interfaces;

public interface IUserRepository
{
    Task<AppUser?> GetUserByEmailAsync(string email);
    Task<AppUser?> GetUserByIdAsync(string userId);
    Task<IdentityResult> CreateUserAsync(AppUser user, string password);
    Task<IdentityResult> UpdateUserAsync(AppUser user);
    Task<IdentityResult> DeleteUserAsync(AppUser user);
    Task<bool> CheckPasswordAsync(AppUser user, string password);
    Task<IdentityResult> ChangePasswordAsync(AppUser user, string currentPassword, string newPassword);
    Task<IList<string>> GetUserRolesAsync(AppUser user);
    Task<IdentityResult> AddToRoleAsync(AppUser user, string role);
    Task<IdentityResult> RemoveFromRoleAsync(AppUser user, string role);
    Task<bool> IsInRoleAsync(AppUser user, string role);
    Task<bool> RoleExistsAsync(string roleName);
    Task<IdentityResult> CreateRoleAsync(string roleName);
    Task<IdentityRole?> GetRoleByNameAsync(string roleName);
    Task<IList<AppUser>> GetUsersInRoleAsync(string roleName);
}