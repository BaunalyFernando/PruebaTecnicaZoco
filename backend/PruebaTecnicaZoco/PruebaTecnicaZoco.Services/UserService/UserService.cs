using Microsoft.EntityFrameworkCore;
using PruebaTecnicaZoco.Repository;
using PruebaTecnicaZoco.Repository.Users;
using PruebaTecnicaZoco.Services.UserService.UserDTO;

namespace PruebaTecnicaZoco.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;

        public UserService(AppDbContext context)
        {
            _context = context;
        }
        public User CreateUser(UserNormalDTO user)
        {
          
            if (string.IsNullOrEmpty(user.Nombre) || string.IsNullOrEmpty(user.Apellido) || string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(user.Password))
            {
                throw new ArgumentException("Por favor ingrese todos los datos de los campos");
            }

            var newUser = new User
            {
                Nombre = user.Nombre,
                Apellido = user.Apellido,
                Email = user.Email,
                Password = user.Password,
                Role = Role.User
            };

            _context.Users.Add(newUser);
            _context.SaveChanges();

            return newUser;
        }

        public User CreateUserByAdmin(UserAdminDTO user)
        {
            if (string.IsNullOrEmpty(user.Nombre) || string.IsNullOrEmpty(user.Apellido) || string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(user.Password))
            {
                throw new ArgumentException("Por favor ingrese todos los datos de los campos");
            }

            var newUser = new User
            {
                Nombre = user.Nombre,
                Apellido = user.Apellido,
                Email = user.Email,
                Password = user.Password,
                Role = user.Role
            };

            _context.Users.Add(newUser);
            _context.SaveChanges();

            return newUser;
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            if (id <= 0) throw new ArgumentException("Por favor ingrese un ID válido");

            var user = await _context.Users.FindAsync(id);

            if (user == null) throw new ArgumentException("No se ha encontrado el usuario");

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Por favor ingrese un ID valido");
            }

            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                throw new ArgumentException("No se ha encontrado el usuario");
            }

            return user;
        }

        public async Task<User> UpdateUserAsync(UserToModifyDTO user)
        {
            if(user == null)
            {
                throw new ArgumentException("Por favor ingrese un usuario valido");
            }

            var existingUser = await GetUserByIdAsync(user.Id);

            if (existingUser == null)
            {
                throw new ArgumentException("No se ha encontrado el usuario");
            }


            existingUser.Nombre = user.Nombre;
            existingUser.Apellido = user.Apellido;
            existingUser.Email = user.Email;
            existingUser.Password = user.Password;

            _context.Users.Update(existingUser);
            await _context.SaveChangesAsync();

            return existingUser;
        }
    }
}
