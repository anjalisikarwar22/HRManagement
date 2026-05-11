using HRManagement.API.DTOs.Auth;

namespace HRManagement.API.Services
{
    public interface IAuthService
    {
        Task<string> RegisterAsync(RegisterDto dto);

        Task<object> LoginAsync(LoginDto dto);

        Task<object> GetCurrentUserAsync(decimal employeeId);
    }

}
