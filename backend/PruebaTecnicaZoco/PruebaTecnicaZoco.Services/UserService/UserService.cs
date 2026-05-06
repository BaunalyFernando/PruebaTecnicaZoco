using Microsoft.EntityFrameworkCore;
using PruebaTecnicaZoco.Common.Exceptions;
using PruebaTecnicaZoco.Repository;
using PruebaTecnicaZoco.Repository.Users;
using PruebaTecnicaZoco.Services.UserService.UserDTO;

namespace PruebaTecnicaZoco.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;

        private readonly ICurrentUserService _currentUser;

        public UserService(AppDbContext context, ICurrentUserService currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }

        public async Task<User> CreateUser(UserNormalDTO user)
        {
            if (string.IsNullOrEmpty(user.Nombre) || string.IsNullOrEmpty(user.Apellido) ||
                string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(user.Password))
            {
                throw new BadRequestException("Por favor ingrese todos los datos de los campos");
            }

            var email = user.Email.ToLower();

            await ValidateUserData(email);

            var hashedPassword = PasswordHasher.HashPassword(user.Password);

            var newUser = new User
            {
                Nombre = user.Nombre,
                Apellido = user.Apellido,
                Email = email,
                Password = hashedPassword,
                Dni = user.Dni,
                Role = Role.User
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            return newUser;
        }

        public async Task<User> CreateUserByAdmin(UserAdminDTO user)
        {
            if (string.IsNullOrEmpty(user.Nombre) || string.IsNullOrEmpty(user.Apellido) ||
                string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(user.Password))
            {
                throw new BadRequestException("Por favor ingrese todos los datos de los campos");
            }

            await ValidateUserData(user.Email);

            var hashedPassword = PasswordHasher.HashPassword(user.Password);

            var newUser = new User
            {
                Nombre = user.Nombre,
                Apellido = user.Apellido,
                Email = user.Email.ToLower(),
                Password = hashedPassword,
                Dni = user.Dni,
                Role = Role.User
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            return newUser;
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            if (id <= 0) throw new BadRequestException("Por favor ingrese un ID válido");

            var user = await GetUserOrThrow(id);
            if (user == null) throw new NotFoundException("No se ha encontrado el usuario");

            if (!_currentUser.IsAdmin && user.Id != _currentUser.UserId)
                throw new UnauthorizedAccessException("No tiene permiso para eliminar este perfil.");

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return true;
        }


        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            if (!_currentUser.IsAdmin)
                throw new UnauthorizedAccessException("Solo los administradores pueden ver todos los usuarios.");

            return await _context.Users.AsNoTracking().ToListAsync();
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            if (id <= 0) throw new BadRequestException("Por favor ingrese un ID válido");

            var user = await _context.Users.FindAsync(id);
            if (user == null) throw new NotFoundException("No se ha encontrado el usuario");

            if (!_currentUser.IsAdmin && user.Id != _currentUser.UserId)
                throw new UnauthorizedAccessException("No tiene permiso para acceder a este perfil.");

            return user;
        }

        public async Task<User> UpdateUserAsync(UserToModifyDTO user)
        {
            if (user == null) throw new BadRequestException("Por favor ingrese un usuario válido");

            var existingUser = await GetUserOrThrow(user.Id);

            if (!_currentUser.IsAdmin && existingUser.Id != _currentUser.UserId)
                throw new UnauthorizedAccessException("No tiene permiso para editar este perfil.");

            if (!string.IsNullOrEmpty(user.Nombre))
                existingUser.Nombre = user.Nombre;

            if (!string.IsNullOrEmpty(user.Password))
                existingUser.Password = PasswordHasher.HashPassword(user.Password);

            if (!string.IsNullOrEmpty(user.Email))
                existingUser.Email = user.Email.ToLower();

            if (!string.IsNullOrEmpty(user.Apellido))
                existingUser.Apellido = user.Apellido;

            if (!string.IsNullOrEmpty(user.Dni))
                existingUser.Dni = user.Dni;

            _context.Users.Update(existingUser);
            await _context.SaveChangesAsync();

            return existingUser;
        }

        public async Task<User> UpdateUserByAdminAsync(UserAdminDTO user)
        {
            if (user == null) throw new BadRequestException("Por favor ingrese un usuario válido");

            var existingUser = await GetUserOrThrow(user.Id);
            if (existingUser == null) throw new NotFoundException("Usuario no encontrado");

            if (!_currentUser.IsAdmin && existingUser.Id != _currentUser.UserId)
                throw new UnauthorizedAccessException("No tiene permiso para editar este perfil.");

            if (!string.IsNullOrEmpty(user.Password))
                existingUser.Password = PasswordHasher.HashPassword(user.Password);

            if (!string.IsNullOrEmpty(user.Apellido))
                existingUser.Apellido = user.Apellido;

            if (!string.IsNullOrEmpty(user.Dni))
                existingUser.Dni = user.Dni;

            if (!string.IsNullOrEmpty(user.Nombre))
                existingUser.Nombre = user.Nombre;

            if (!string.IsNullOrEmpty(user.Email))
                existingUser.Email = user.Email.ToLower();

            existingUser.Role = user.Role;

            await _context.SaveChangesAsync();

            return existingUser;
        }
        private async Task ValidateUserData(string email)
        {
            var exists = await _context.Users.AnyAsync(u => u.Email == email);
            if (exists)
                throw new BadRequestException("Ya existe un usuario...");
        }
        private async Task<User> GetUserOrThrow(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                throw new NotFoundException("Usuario no encontrado");

            return user;
        }
    }
}
