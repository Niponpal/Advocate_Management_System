using System.Security.Claims;

namespace AMS.Helper;

public interface ISignInHelper
{
    public long? UserId { get; }
    public string Email { get; }
    public string Fullname { get; }
    public string MobileNumber { get; }
    public string Username { get; }
    public List<string> Roles { get; }
    public bool IsAuthenticated { get; }
    public string AccessToken { get; }
    public DateTimeOffset JwtExpiresAt { get; }
    public string RequestOrigin { get; }
    public long? MemberId { get; }
}
public class SignInHelper : ISignInHelper
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public SignInHelper(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public long? UserId => long.TryParse(_httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier), out var id) ? id : null;
    public string Email => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Email);
    public string Username => _httpContextAccessor.HttpContext?.User?.Identity?.Name;
    public List<string> Roles => _httpContextAccessor.HttpContext?.User?.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList() ?? new();
    public bool IsAuthenticated => _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;
    public string AccessToken => _httpContextAccessor.HttpContext?.Request?.Headers["Authorization"].ToString()?.Split(' ')[1];
    public DateTimeOffset JwtExpiresAt => DateTimeOffset.UtcNow; // calculate from token if needed
    public string RequestOrigin => _httpContextAccessor.HttpContext?.Request?.Headers["Origin"].ToString();

    public string Fullname => _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name);
    public string MobileNumber => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.MobilePhone);

    public long? MemberId =>
    long.TryParse(_httpContextAccessor.HttpContext?.User?.FindFirst("MemberId")?.Value, out var id)
        ? id
        : null;

}