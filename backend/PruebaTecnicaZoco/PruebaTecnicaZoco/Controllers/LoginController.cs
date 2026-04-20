using Microsoft.AspNetCore.Mvc;
using PruebaTecnicaZoco.Services.LoginService;
using PruebaTecnicaZoco.Services.LoginService.LoginDTO;
using PruebaTecnicaZoco.Common.Exceptions;

[ApiController]
[Route("auth/")]
public class LoginController : ControllerBase
{
    private readonly ILogger<LoginController> _logger;
    private readonly ILoginService _loginService;

    public LoginController(ILogger<LoginController> logger, ILoginService loginService)
    {
        _logger = logger;
        _loginService = loginService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto request)
    {
        var loginResult = await _loginService.LoginAsync(request);

        return Ok(loginResult);
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout([FromBody] SessionLogDTO session)
    {
        await _loginService.LogoutAsync(session);
        return Ok("Sesión cerrada correctamente.");
    }
}
