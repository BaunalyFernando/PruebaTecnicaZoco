using Microsoft.EntityFrameworkCore;
using PruebaTecnicaZoco.Repository;
using PruebaTecnicaZoco.Repository.Addresses;
using PruebaTecnicaZoco.Services.AddressService.AddressesDTO;
using PruebaTecnicaZoco.Common.Exceptions;
using Microsoft.AspNetCore.Http;
using PruebaTecnicaZoco.Repository.Studies;
using System.Security.Claims;

namespace PruebaTecnicaZoco.Services.AddressService
{
    public class AddressService : IAddressService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AppDbContext _context;

        public AddressService(AppDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Address> CreateAddressAsync(AddressDTO address)
        {
            if (string.IsNullOrEmpty(address.Calle) || string.IsNullOrEmpty(address.Numero) || string.IsNullOrEmpty(address.Ciudad))
                throw new BadRequestException("Por favor ingrese todos los datos de los campos");

            var userExists = await _context.Users.AnyAsync(u => u.Id == address.UserId);
            if (!userExists)
                throw new NotFoundException("El usuario no existe.");

            var newAddress = new Address
            {
                Calle = address.Calle,
                Numero = address.Numero,
                Ciudad = address.Ciudad,
                UserId = address.UserId
            };

            _context.Addresses.Add(newAddress);
            await _context.SaveChangesAsync();

            return newAddress;
        }

        public async Task<bool> DeleteAddressAsync(int id)
        {
            if (id <= 0)
                throw new BadRequestException("El id no puede ser menor o igual a 0");

            var address = await GetAddressByIdAsync(id);

            _context.Addresses.Remove(address);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Address>> GetAllAddressesAsync()
        {
            if (IsAdmin())
            {
                return await _context.Addresses.ToListAsync();
            }

            var userId = GetCurrentUserId();
            return await _context.Addresses.Where(a => a.UserId == userId).ToListAsync();
        }

        public async Task<Address> GetAddressByIdAsync(int id)
        {
            if (id <= 0)
                throw new BadRequestException("El ID debe ser mayor que cero.");

            var address = await _context.Addresses.FindAsync(id);
            if (address == null)
                throw new NotFoundException("Dirección no encontrada.");

            if (!IsAdmin() && address.UserId != GetCurrentUserId())
                throw new UnauthorizedAccessException("No tiene permiso para acceder a este estudio.");

            return address;
        }

        public async Task<Address> UpdateAddressAsync(AddressModifyDTO address)
        {
            if (address == null || address.Id <= 0)
                throw new BadRequestException("Datos de dirección inválidos.");

            var existingAddress = await _context.Addresses.FindAsync(address.Id);
            if (existingAddress == null)
                throw new NotFoundException("Dirección no encontrada.");

           
            if (!IsAdmin() && existingAddress.UserId != GetCurrentUserId())
                throw new UnauthorizedAccessException("No tiene permiso para modificar esta dirección.");

            existingAddress.Calle = address.Calle;
            existingAddress.Numero = address.Numero;
            existingAddress.Ciudad = address.Ciudad;

            _context.Addresses.Update(existingAddress);
            await _context.SaveChangesAsync();

            return existingAddress;
        }

        private int GetCurrentUserId()
        {
            return int.Parse(_httpContextAccessor.HttpContext!.User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        }

        private bool IsAdmin()
        {
            return _httpContextAccessor.HttpContext!.User.IsInRole("Admin");
        }

        public async Task<IEnumerable<Address>> GetAddressesByUserIdAsync(int userId)
        {
            var addresses = await _context.Addresses
                .Where(a => a.UserId == userId)
                .Select(a => new Address
                {
                    Id = a.Id,
                    Calle = a.Calle,
                    Numero = a.Numero,
                    Ciudad = a.Ciudad,
                    UserId = a.UserId
                })
                .ToListAsync();

            return addresses;
        }

    }
}
