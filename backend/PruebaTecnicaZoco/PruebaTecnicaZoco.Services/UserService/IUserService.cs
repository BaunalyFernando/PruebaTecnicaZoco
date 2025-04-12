using PruebaTecnicaZoco.Services.UserService.UserDTO;

namespace PruebaTecnicaZoco.Services.UserService
{
    public interface IUserService
    {
        Task<User> CreateUser(UserNormalDTO user);
        Task<User> CreateUserByAdmin(UserAdminDTO user);
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User> GetUserByIdAsync(int id);
        Task<User> UpdateUserAsync(UserToModifyDTO user);
        Task<bool> DeleteUserAsync(int id);
    }
}
