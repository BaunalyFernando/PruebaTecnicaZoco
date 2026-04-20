using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PruebaTecnicaZoco.Common.Exceptions;
using PruebaTecnicaZoco.Repository;
using PruebaTecnicaZoco.Repository.SessionLogs;
using PruebaTecnicaZoco.Repository.Users;
using PruebaTecnicaZoco.Services.LoginService.LoginDTO;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PruebaTecnicaZoco.Services.LoginService
{
    public class LoginService : ILoginService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public LoginService(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<LoginResponseDto> LoginAsync(LoginDto loginDto)
        {
            var email = loginDto.Email.ToLower();

            var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == email);

            if (user == null || !PasswordHasher.VerifyPassword(loginDto.Password, user.Password))
            {
                throw new UnauthorizedException("Credenciales inválidas.");
            }

            var activeSession = await _context.SessionLogs
                                .FirstOrDefaultAsync(s => s.UserId == user.Id && s.FechaFin == null);
            if (activeSession != null)
            {
                throw new InvalidOperationException("Ya existe una sesión activa para este usuario.");
            }

            var token = GenerateToken(user.Id, user.Email, user.Role);

            var newSession = new SessionLog
            {
                UserId = user.Id,
                FechaInicio = DateTime.UtcNow,
                FechaFin = null
            };

            var result = new LoginResponseDto
            {
                Token = token,
                Id = user.Id,
                Email = user.Email,
                Role = user.Role
            };

            _context.SessionLogs.Add(newSession);
            await _context.SaveChangesAsync();

            return result;
        }

        public async Task<bool> LogoutAsync(SessionLogDTO session)
        {
            if (session.UserId == 0)
            {
                throw new BadRequestException("Por favor ingrese un userId válido");
            }

            var userExists = await _context.Users.AnyAsync(u => u.Id == session.UserId);
            if (!userExists)
            {
                throw new NotFoundException("Usuario no encontrado.");
            }

            var existingSession = await _context.SessionLogs
                 .FirstOrDefaultAsync(s => s.UserId == session.UserId && s.FechaFin == null);

            if (existingSession == null)
            {
                throw new BadRequestException("No hay una sesión activa para este usuario.");
            }

            existingSession.FechaFin = DateTime.UtcNow;
            _context.SessionLogs.Update(existingSession);
            await _context.SaveChangesAsync();
            return true;
        }

        private string GenerateToken(int userId, string email, Role role)
        {
            var key = _configuration["Jwt:Key"];
            var keyBytes = Encoding.UTF8.GetBytes(key);
            var securityKey = new SymmetricSecurityKey(keyBytes);
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
            new Claim(ClaimTypes.Name, email),
            new Claim(ClaimTypes.Email, email),
            new Claim(ClaimTypes.Role, role.ToString())
        };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
