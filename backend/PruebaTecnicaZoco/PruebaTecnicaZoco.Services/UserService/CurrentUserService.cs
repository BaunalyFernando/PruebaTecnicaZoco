using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace PruebaTecnicaZoco.Services.UserService
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor accessor)
        {
            _httpContextAccessor = accessor;
        }

        public int UserId =>
            int.Parse(_httpContextAccessor.HttpContext!.User
                .FindFirst(ClaimTypes.NameIdentifier)!.Value);

        public bool IsAdmin =>
            _httpContextAccessor.HttpContext!.User.IsInRole("Admin");
    }
}
