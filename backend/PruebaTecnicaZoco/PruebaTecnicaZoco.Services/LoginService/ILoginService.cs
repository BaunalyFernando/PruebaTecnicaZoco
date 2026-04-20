using PruebaTecnicaZoco.Repository.SessionLogs;
using PruebaTecnicaZoco.Services.LoginService.LoginDTO;

namespace PruebaTecnicaZoco.Services.LoginService
{
    public interface ILoginService
    {
        Task<LoginResponseDto> LoginAsync(LoginDto loginDto);
        Task<bool> LogoutAsync(SessionLogDTO session);
    }
}
