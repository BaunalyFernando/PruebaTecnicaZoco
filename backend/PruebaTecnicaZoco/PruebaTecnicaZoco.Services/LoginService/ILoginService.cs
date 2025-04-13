using PruebaTecnicaZoco.Repository.SessionLogs;
using PruebaTecnicaZoco.Services.LoginService.LoginDTO;

namespace PruebaTecnicaZoco.Services.LoginService
{
    public interface ILoginService
    {
        Task<bool> LoginAsync(SessionLogDTO session);
        Task<bool> LogoutAsync(SessionLogDTO session);
    }
}
