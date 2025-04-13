namespace PruebaTecnicaZoco.Services.AddressService.AddressesDTO
{
    public class AddressModifyDTO
    {
        public int Id { get; set; }
        public string Calle { get; set; }
        public string Numero { get; set; }
        public string Ciudad { get; set; }

        public int UserId { get; set; }
    }
}
