using Span.Culturio.Api.Models.DTOs;

namespace Span.Culturio.Api.Services.Interfaces
{
    public interface IAuthService
    {
        Task<bool> RegisterAsync(RegisterUserDto dto);
        Task<string?> LoginAsync(LoginDto dto);
    }
}
