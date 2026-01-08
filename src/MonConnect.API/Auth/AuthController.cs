using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
//using MonConnect.Application.Auth.Commands; // Asegúrate de que esta sea la ruta de tu LoginCommand
//using MonConnect.Application.Auth.DTOs;     // Para el AuthResponseDto

namespace MonConnect.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Autentica a un usuario y devuelve un Token JWT.
    /// </summary>
    /// <param name="command">Correo y Contraseña</param>
    /// <returns>Token de acceso y datos del usuario</returns>
    [AllowAnonymous]
    [HttpPost("login")]
    [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginCommand command)
    {
        try
        {
            var response = await _mediator.Send(command);
            
            if (response == null)
                return Unauthorized(new { message = "Credenciales inválidas" });

            return Ok(response);
        }
        catch (Exception ex)
        {
            // Aquí puedes loguear el error si tienes un Logger
            return Unauthorized(new { message = ex.Message });
        }
    }
}