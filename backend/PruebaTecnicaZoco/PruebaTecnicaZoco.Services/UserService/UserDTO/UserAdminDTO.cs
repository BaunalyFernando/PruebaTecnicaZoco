using PruebaTecnicaZoco.Repository.Users;

namespace PruebaTecnicaZoco.Services.UserService.UserDTO
{
    public class UserAdminDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public Role Role { get; set; }
    }
}
