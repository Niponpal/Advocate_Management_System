using AMS.Models.Auth;
using Microsoft.AspNetCore.Identity;
using static AMS.Auth_IdentityModel.IdentityModel;

namespace AMS;

public interface IAuthService
{
    Task<RegistrationResponse> Register(RegisterViewModel model);
}

public class AuthService : IAuthService
{
    private readonly UserManager<User> _userManager;

    public AuthService(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<RegistrationResponse> Register(RegisterViewModel request)
    {
        var existingUser = await _userManager.FindByEmailAsync(request.Email);
        if (existingUser != null)
        {
            return new RegistrationResponse
            {
                Success = false,
                Errors = new() { $"Email '{request.Email}' is already registered." }
            };
        }

        var user = new User
        {
            Email = request.Email,
            UserName = request.Email,
            PhoneNumber = request.PhoneNumber,
            FullName = request.FullName,
            Address = request.Address,
            EmailConfirmed = true,
            SecurityStamp = Guid.NewGuid().ToString()
        };

        var result = await _userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            return new RegistrationResponse
            {
                Success = false,
                Errors = result.Errors.Select(e => e.Description).ToList()
            };
        }

        await _userManager.AddToRoleAsync(user, request.AccountType);

        return new RegistrationResponse
        {
            Success = true,
            UserId = user.Id
        };
    }
}