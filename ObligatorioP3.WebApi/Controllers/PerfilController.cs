using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ObligatorioP3.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PerfilController : ControllerBase
    {
        private readonly ILogger<PerfilController> _logger;
        public PerfilController(ILogger<PerfilController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Consulta el perfil del usuario autenticado.
        /// </summary>
        /// <remarks>
        /// Devuelve identificador, nombre y rol obtenidos de los claims del JWT.
        /// </remarks>
        /// <response code="200">Datos del perfil.</response>
        /// <response code="401">No autenticado.</response>
        /// <response code="500">Error interno del servidor.</response>
        [HttpGet]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Get()
        {
            try
            {
                return Ok(new
                {
                    Id = User.FindFirst(ClaimTypes.Sid)?.Value,
                    Nombre = User.FindFirst(ClaimTypes.Name)?.Value,
                    Rol = User.FindFirst(ClaimTypes.Role)?.Value
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al consultar el perfil.");
                return StatusCode(500, new { mensaje = "Ocurrió un error inesperado. Intente nuevamente más tarde." });
            }
        }
    }
}
