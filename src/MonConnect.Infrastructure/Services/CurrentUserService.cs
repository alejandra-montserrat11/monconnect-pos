
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using MonConnect.Application.Common.Interfaces;

namespace MonConnect.Infrastructure.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    // Extrae el ID del claim "sub" o "NameIdentifier" del Token
    public string? UserId => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
}