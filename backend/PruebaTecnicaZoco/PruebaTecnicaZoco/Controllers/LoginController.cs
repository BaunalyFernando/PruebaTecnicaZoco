using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PruebaTecnicaZoco.Repository;
using PruebaTecnicaZoco.Services.LoginService;
using PruebaTecnicaZoco.Services.LoginService.LoginDTO;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using PruebaTecnicaZoco.Repository.Users;
using PruebaTecnicaZoco.Repository.SessionLogs;
using PruebaTecnicaZoco.Common.Exceptions;

[ApiController]
[Route("auth/")]
public class LoginController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly AppDbContext _context;
    private readonly ILogger<LoginController> _logger;
    private readonly ILoginService _loginService;

    public LoginController(IConfiguration configuration, AppDbContext context, ILogger<LoginController> logger, ILoginService loginService)
    {
        _configuration = configuration;
        _context = context;
        _logger = logger;
        _loginService = loginService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDTO request)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == request.Email.ToLower());

        if (user == null || !PasswordHasher.VerifyPassword(request.Password, user.Password))
        {
            throw new UnauthorizedException("Credenciales inválidas.");
        }

        var token = GenerateToken(user.Id, user.Email, user.Role);

        var sessionLog = new SessionLog
        {
            UserId = user.Id,
            FechaInicio = DateTime.Now,
            FechaFin = null
        };

        await _loginService.LoginAsync(sessionLog);

        return Ok(new
        {
            token,
            id = user.Id,
            email = user.Email,
            role = user.Role.ToString()
        });
    }



    [HttpPost("logout")]
    public async Task<IActionResult> Logout([FromBody] SessionLogDTO session)
    {
        if (session.UserId == 0)
        {
            throw new BadRequestException("Por favor ingrese un userId válido");
        }

        await _loginService.LogoutAsync(session);
        return Ok("Sesión cerrada correctamente.");
    }


    private string GenerateToken(int userId, string email, Role role)
    {
        var key = _configuration["Jwt:Key"];
        var keyBytes = Encoding.UTF8.GetBytes(key);
        var securityKey = new SymmetricSecurityKey(keyBytes);
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
            new Claim(ClaimTypes.Name, email),
            new Claim(ClaimTypes.Email, email),
            new Claim(ClaimTypes.Role, role.ToString())
        };

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
