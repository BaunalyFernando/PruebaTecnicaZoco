using PruebaTecnicaZoco.Repository.Addresses;
using PruebaTecnicaZoco.Services.AddressService.AddressesDTO;

namespace PruebaTecnicaZoco.Services.AddressService
{
    public interface IAddressService
    {
        Task<Address> CreateAddressAsync(AddressDTO address);
        Task<Address> GetAddressByIdAsync(int id);
        Task<IEnumerable<Address>> GetAllAddressesAsync();
        Task<Address> UpdateAddressAsync(AddressModifyDTO address);
        Task<bool> DeleteAddressAsync(int id);
    }
}
