﻿using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PruebaTecnicaZoco.Common.Exceptions;
using PruebaTecnicaZoco.Repository;
using PruebaTecnicaZoco.Repository.Studies;
using PruebaTecnicaZoco.Services.StudyService.StudiesDto;
using PruebaTecnicaZoco.Services.StudyService.StudiesDTO;
using System.Security.Claims;

namespace PruebaTecnicaZoco.Services.StudyService
{
    public class StudyService : IStudyService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AppDbContext _context;

        public StudyService(AppDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }


        public async Task<Study> CreateStudyAsync(StudyDTO study)
        {
            if (string.IsNullOrWhiteSpace(study.Nombre) || string.IsNullOrWhiteSpace(study.Descripcion) || study.UserId <= 0)
                throw new BadRequestException("Por favor ingrese todos los datos de los campos");

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
            return await _context.Studies.ToListAsync();
        }

        public async Task<Study> GetStudyByIdAsync(int id)
        {
            if (id <= 0)
                throw new BadRequestException("El id no puede ser menor o igual a 0");

            var study = await _context.Studies.FindAsync(id);
            if (study == null)
                throw new NotFoundException("No se encontró el estudio con el id proporcionado");

            if (!IsAdmin() && study.UserId != GetCurrentUserId())
                throw new UnauthorizedAccessException("No tiene permiso para acceder a este estudio.");

            return study;
        }

        public async Task<Study> UpdateStudyAsync(StudyModifyDTO study)
        {
            if (string.IsNullOrWhiteSpace(study.Nombre) || string.IsNullOrWhiteSpace(study.Descripcion) || study.UserId <= 0)
                throw new BadRequestException("Por favor ingrese todos los datos de los campos");

            var existingStudy = await GetStudyByIdAsync(study.Id);
            existingStudy.Nombre = study.Nombre;
            existingStudy.Descripcion = study.Descripcion;
            existingStudy.UserId = study.UserId;

            _context.Studies.Update(existingStudy);
            await _context.SaveChangesAsync();
            return existingStudy;
        }

        private int GetCurrentUserId()
        {
            return int.Parse(_httpContextAccessor.HttpContext!.User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        }

        private bool IsAdmin()
        {
            return _httpContextAccessor.HttpContext!.User.IsInRole("Admin");
        }

        public async Task<IEnumerable<Study>> GetStudiesByUserIdAsync(int userId)
        {
            var studies = await _context.Studies
                .Where(s => s.UserId == userId)
                .Select(s => new Study
                {
                    Id = s.Id,
                    Nombre = s.Nombre,
                    Descripcion = s.Descripcion,
                    UserId = s.UserId
                })
                .ToListAsync();

            return studies;
        }

    }
}
