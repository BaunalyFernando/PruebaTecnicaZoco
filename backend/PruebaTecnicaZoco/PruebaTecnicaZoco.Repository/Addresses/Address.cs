using PruebaTecnicaZoco.Repository.Users;

namespace PruebaTecnicaZoco.Repository.Addresses
{
    public class Address
    {
        public int Id { get; set; }
        public string Calle { get; set; }
        public string Numero { get; set; }
        public string Ciudad { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }
    }
}
