using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PruebaTecnicaZoco.Services.AddressService;
using PruebaTecnicaZoco.Services.AddressService.AddressesDTO;

namespace PruebaTecnicaZoco.Controllers
{
    [ApiController]
    [Route("api/Addresses")]
    public class AddressController : ControllerBase
    {
        private readonly IAddressService _addressService;

        public AddressController(IAddressService addressService)
        {
            _addressService = addressService;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateAddress([FromBody] AddressDTO address)
        {
            if (string.IsNullOrEmpty(address.Calle) || string.IsNullOrEmpty(address.Ciudad) || string.IsNullOrEmpty(address.Numero))
            {
                return BadRequest("Por favor ingrese todos los datos de los campos");
            }

            try
            {
                var createdAddress = await _addressService.CreateAddressAsync(address);
                return CreatedAtAction(nameof(GetAddressById), new { id = createdAddress.Id }, createdAddress);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllAddresses()
        {
            try
            {
                var addresses = await _addressService.GetAllAddressesAsync();
                return Ok(addresses);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetAddressById(int id)
        {
            try
            {
                var address = await _addressService.GetAddressesByUserIdAsync(id);
                return Ok(address);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteAddress(int id)
        {
            try
            {
                var result = await _addressService.DeleteAddressAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> UpdateAddress([FromBody] AddressModifyDTO address)
        {
            if (string.IsNullOrEmpty(address.Calle) || string.IsNullOrEmpty(address.Ciudad) || string.IsNullOrEmpty(address.Numero))
            {
                return BadRequest("Por favor ingrese todos los datos de los campos");
            }

            try
            {
                var updatedAddress = await _addressService.UpdateAddressAsync(address);
                return Ok(updatedAddress);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> GetMyAddresses()
        {
            var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);
            var addresses = await _addressService.GetAddressesByUserIdAsync(userId);
            return Ok(addresses);
        }

    }
}
