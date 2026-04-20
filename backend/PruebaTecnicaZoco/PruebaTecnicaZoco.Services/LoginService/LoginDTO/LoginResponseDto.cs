using PruebaTecnicaZoco.Repository.Users;

namespace PruebaTecnicaZoco.Services.LoginService.LoginDTO
{
    public class LoginResponseDto
    {
        public string Token { get; set; }
        public int Id { get; set; }
        public string Email { get; set; }
        public Role Role { get; set; }
    }
}
