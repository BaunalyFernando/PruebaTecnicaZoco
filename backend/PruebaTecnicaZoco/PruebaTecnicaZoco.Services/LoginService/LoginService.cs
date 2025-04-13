using Microsoft.EntityFrameworkCore;
using PruebaTecnicaZoco.Common.Exceptions;
using PruebaTecnicaZoco.Repository;
using PruebaTecnicaZoco.Repository.SessionLogs;
using PruebaTecnicaZoco.Services.LoginService.LoginDTO;

namespace PruebaTecnicaZoco.Services.LoginService
{
    public class LoginService : ILoginService
    {
        private readonly AppDbContext _context;

        public LoginService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> LoginAsync(SessionLogDTO session)
        {
            if (session.UserId == 0)
            {
                throw new BadRequestException("Por favor ingrese un userId válido.");
            }

            var userExists = await _context.Users.AnyAsync(u => u.Id == session.UserId);
            if (!userExists)
            {
                throw new NotFoundException("Usuario no encontrado.");
            }

            var activeSession = await _context.SessionLogs
                                .FirstOrDefaultAsync(s => s.UserId == session.UserId && s.FechaFin == null);
            if (activeSession != null)
            {
                throw new InvalidOperationException("Ya existe una sesión activa para este usuario.");
            }

            var newSession = new SessionLog
            {
                UserId = session.UserId,
                FechaInicio = DateTime.UtcNow,
                FechaFin = null
            };

            _context.SessionLogs.Add(newSession);
            await _context.SaveChangesAsync();

            return true;
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
                throw new InvalidOperationException("No hay una sesión activa para este usuario.");
            }

            existingSession.FechaFin = DateTime.Now;
            _context.SessionLogs.Update(existingSession);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
