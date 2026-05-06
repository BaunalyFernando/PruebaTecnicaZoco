namespace PruebaTecnicaZoco.Services.UserService
{
    public interface ICurrentUserService
    {
        int UserId { get; }
        bool IsAdmin { get; }
    }
}
