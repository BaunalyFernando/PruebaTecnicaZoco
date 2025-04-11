
using PruebaTecnicaZoco.Repository.Users;

namespace PruebaTecnicaZoco.Repository.SessionLogs
{
    public class SessionLog
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime FechaInicio { get; set; }

        public DateTime? FechaFin { get; set; }

        public User User { get; set; }
    }
}
