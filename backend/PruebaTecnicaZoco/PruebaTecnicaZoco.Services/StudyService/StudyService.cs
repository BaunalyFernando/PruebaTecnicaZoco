using Microsoft.EntityFrameworkCore;
using PruebaTecnicaZoco.Repository;
using PruebaTecnicaZoco.Repository.Studies;
using PruebaTecnicaZoco.Services.StudyService.StudiesDto;
using PruebaTecnicaZoco.Services.StudyService.StudiesDTO;

namespace PruebaTecnicaZoco.Services.StudyService
{
    public class StudyService : IStudyService
    {
        private readonly AppDbContext _context;

        public StudyService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Study> CreateStudyAsync(StudyDTO study)
        {
            if (string.IsNullOrEmpty(study.Nombre) || string.IsNullOrEmpty(study.Descripcion) || study.UserId <= 0)
            {
                throw new InvalidOperationException("Por favor ingrese todos los datos de los campos");
            }

            var userExists = await _context.Users.AnyAsync(u => u.Id == study.UserId);
            if (!userExists)
            {
                throw new ArgumentException("El usuario especificado no existe.");
            }

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
            {
                throw new InvalidOperationException("El id no puede ser menor o igual a 0");
            }

            var study = await GetStudyByIdAsync(id);

           
            _context.Studies.Remove(study);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Study>> GetAllStudiesAsync()
        {
            return await _context.Studies.ToListAsync();
        }

        public async Task<Study> GetStudyByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new InvalidOperationException("El id no puede ser menor o igual a 0");
            }

            var study = await _context.Studies.FindAsync(id);

            if (study == null)
            {
                throw new KeyNotFoundException("No se encontró el estudio con el id proporcionado");
            }

            return study;
        }

        public async Task<Study> UpdateStudyAsync(StudyModifyDTO study)
        {
            if (string.IsNullOrEmpty(study.Nombre) || string.IsNullOrEmpty(study.Descripcion) || study.UserId <= 0)
            {
                throw new ArgumentException("Por favor ingrese todos los datos de los campos");
            }

            var existingStudy = await GetStudyByIdAsync(study.Id);

            existingStudy.Nombre = study.Nombre;
            existingStudy.Descripcion = study.Descripcion;
            existingStudy.UserId = study.UserId;

            _context.Studies.Update(existingStudy);
            await _context.SaveChangesAsync();
            return existingStudy;
        }
    }
}
