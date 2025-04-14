using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PruebaTecnicaZoco.Common.Exceptions;
using PruebaTecnicaZoco.Repository;
using PruebaTecnicaZoco.Repository.Users;
using PruebaTecnicaZoco.Services.UserService.UserDTO;
using System.Security.Claims;

namespace PruebaTecnicaZoco.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(AppDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<User> CreateUser(UserNormalDTO user)
        {
            if (string.IsNullOrEmpty(user.Nombre) || string.IsNullOrEmpty(user.Apellido) ||
                string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(user.Password))
            {
                throw new BadRequestException("Por favor ingrese todos los datos de los campos");
            }

            var email = user.Email.ToLower();

            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == email);

            if (existingUser != null)
            {
                throw new BadRequestException("Ya existe un usuario con este correo electrónico");
            }

            var hashedPassword = PasswordHasher.HashPassword(user.Password);

            var newUser = new User
            {
                Nombre = user.Nombre,
                Apellido = user.Apellido,
                Email = email,
                Password = hashedPassword,
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

            var email = user.Email.ToLower();

            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == email);

            if (existingUser != null)
            {
                throw new BadRequestException("Ya existe un usuario con este correo electrónico");
            }

            var hashedPassword = PasswordHasher.HashPassword(user.Password);

            var newUser = new User
            {
                Nombre = user.Nombre,
                Apellido = user.Apellido,
                Email = email,
                Password = hashedPassword,
                Role = Role.User
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            return newUser;
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            if (id <= 0) throw new BadRequestException("Por favor ingrese un ID válido");

            var user = await _context.Users.FindAsync(id);
            if (user == null) throw new NotFoundException("No se ha encontrado el usuario");

            if (!IsAdmin() && user.Id != GetCurrentUserId())
                throw new UnauthorizedAccessException("No tiene permiso para eliminar este perfil.");

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return true;
        }


        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            if (!IsAdmin())
                throw new UnauthorizedAccessException("Solo los administradores pueden ver todos los usuarios.");

            return await _context.Users.ToListAsync();
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            if (id <= 0) throw new BadRequestException("Por favor ingrese un ID válido");

            var user = await _context.Users.FindAsync(id);
            if (user == null) throw new NotFoundException("No se ha encontrado el usuario");

            if (!IsAdmin() && user.Id != GetCurrentUserId())
                throw new UnauthorizedAccessException("No tiene permiso para acceder a este perfil.");

            return user;
        }

        public async Task<User> UpdateUserAsync(UserToModifyDTO user)
        {
            if (user == null) throw new BadRequestException("Por favor ingrese un usuario válido");

            var existingUser = await _context.Users.FindAsync(user.Id);
            if (existingUser == null) throw new NotFoundException("Usuario no encontrado");

            if (!IsAdmin() && existingUser.Id != GetCurrentUserId())
                throw new UnauthorizedAccessException("No tiene permiso para editar este perfil.");

            existingUser.Nombre = user.Nombre;
            existingUser.Apellido = user.Apellido;
            existingUser.Email = user.Email;
            existingUser.Password = user.Password;

            _context.Users.Update(existingUser);
            await _context.SaveChangesAsync();

            return existingUser;
        }

        private int GetCurrentUserId()
        {
            return int.Parse(_httpContextAccessor.HttpContext!.User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        }

        private bool IsAdmin()
        {
            return _httpContextAccessor.HttpContext!.User.IsInRole("Admin");
        }

    }
}
