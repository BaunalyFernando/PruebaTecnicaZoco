using PruebaTecnicaZoco.Services.UserService.UserDTO;

namespace PruebaTecnicaZoco.Services.UserService
{
    public interface IUserService
    {
        User CreateUser(UserNormalDTO user);
        User CreateUserByAdmin(UserAdminDTO user);
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User> GetUserByIdAsync(int id);
        Task<User> UpdateUserAsync(UserToModifyDTO user);
        Task<bool> DeleteUserAsync(int id);
    }
}
