using PruebaTecnicaZoco.Repository.Addresses;
using PruebaTecnicaZoco.Repository.SessionLogs;
using PruebaTecnicaZoco.Repository.Studies;
using PruebaTecnicaZoco.Repository.Users;

public class User
{
    public int Id { get; set; }
    public string Nombre { get; set; }
    public string Apellido { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public Role Role { get; set; }

    public ICollection<Address> Addresses { get; set; } = new List<Address>();
    public ICollection<Study> Studies { get; set; } = new List<Study>();
    public ICollection<SessionLog> SessionLogs { get; set; }
}
