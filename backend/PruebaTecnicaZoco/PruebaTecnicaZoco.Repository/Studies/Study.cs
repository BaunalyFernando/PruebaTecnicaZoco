using PruebaTecnicaZoco.Repository.Users;

namespace PruebaTecnicaZoco.Repository.Studies
{
    public class Study
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }
    }
}
