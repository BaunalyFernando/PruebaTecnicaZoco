using PruebaTecnicaZoco.Repository.SessionLogs;
using PruebaTecnicaZoco.Services.LoginService.LoginDTO;

namespace PruebaTecnicaZoco.Services.LoginService
{
    public interface ILoginService
    {
        Task<bool> LoginAsync(SessionLog session);
        Task<bool> LogoutAsync(SessionLogDTO session);
    }
}
