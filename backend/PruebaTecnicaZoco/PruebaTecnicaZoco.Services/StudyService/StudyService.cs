using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PruebaTecnicaZoco.Common.Exceptions;
using PruebaTecnicaZoco.Repository;
using PruebaTecnicaZoco.Repository.Studies;
using PruebaTecnicaZoco.Services.StudyService.StudiesDto;
using PruebaTecnicaZoco.Services.StudyService.StudiesDTO;
using PruebaTecnicaZoco.Services.UserService;
using System.Security.Claims;

namespace PruebaTecnicaZoco.Services.StudyService
{
    public class StudyService : IStudyService
    {
        private readonly ICurrentUserService _currentUser;
        private readonly AppDbContext _context;

        public StudyService(AppDbContext context, ICurrentUserService currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }


        public async Task<Study> CreateStudyAsync(StudyDTO study)
        {
            if (!_currentUser.IsAdmin && study.UserId != _currentUser.UserId)
                throw new UnauthorizedAccessException("No puede crear estudios para otro usuario");

            ValidateStudy(study.Nombre, study.Descripcion, study.UserId);

            var userExists = await _context.Users.AnyAsync(u => u.Id == study.UserId);
            if (!userExists)
                throw new NotFoundException("El usuario especificado no existe.");

            var newStudy = new Study
            {
                Nombre = study.Nombre,
                Descripcion = study.Descripcion,
                UserId = study.UserId
            };

            _context.Studies.Add(newStudy);
            await _context.SaveChangesAsync();
            return newStudy;
        }

        public async Task<bool> DeleteStudyAsync(int id)
        {
            if (id <= 0)
                throw new BadRequestException("El id no puede ser menor o igual a 0");

            var study = await GetStudyByIdAsync(id);
            _context.Studies.Remove(study);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Study>> GetAllStudiesAsync()
        {
            var query = _context.Studies.AsQueryable();

            if (!_currentUser.IsAdmin)
                query = query.Where(s => s.UserId == _currentUser.UserId);

            return await query.AsNoTracking().ToListAsync();
        }

        public async Task<Study> GetStudyByIdAsync(int id)
        {
            if (id <= 0)
                throw new BadRequestException("El id no puede ser menor o igual a 0");

            var study = await _context.Studies.AsNoTracking().FirstOrDefaultAsync(s => s.Id == id);

            if (study == null)
                throw new NotFoundException("No se encontró el estudio con el id proporcionado");

            if (!_currentUser.IsAdmin && study.UserId != _currentUser.UserId)
                throw new UnauthorizedAccessException("No tiene permiso para acceder a este estudio.");

            return study;
        }

        public async Task<Study> UpdateStudyAsync(StudyModifyDTO study)
        {
            var studyValidation = new StudyDTO
            {
                Nombre = study.Nombre,
                Descripcion = study.Descripcion,
                UserId = study.UserId
            };
            ValidateStudy(study.Nombre, study.Descripcion, study.UserId);

            var existingStudy = await GetStudyByIdAsync(study.Id);
            existingStudy.Nombre = study.Nombre;
            existingStudy.Descripcion = study.Descripcion;
            existingStudy.UserId = study.UserId;

            _context.Studies.Update(existingStudy);
            await _context.SaveChangesAsync();
            return existingStudy;
        }

        public async Task<IEnumerable<Study>> GetStudiesByUserIdAsync(int userId)
        {
            if (!_currentUser.IsAdmin && userId != _currentUser.UserId)
                throw new UnauthorizedAccessException("No tiene permisos para ver este estudio");
            var studies = await _context.Studies
                .Where(s => s.UserId == userId)
                .AsNoTracking()
                .ToListAsync();

            return studies;
        }

        private void ValidateStudy(string nombre, string descripcion, int userId)
        {
            if (string.IsNullOrWhiteSpace(nombre) || string.IsNullOrWhiteSpace(descripcion) || userId <= 0)
                throw new BadRequestException("Por favor ingrese todos los datos de los campos");
        }
    }
}
