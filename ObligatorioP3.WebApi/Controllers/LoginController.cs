using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ObligatorioP3.LogicaAplicacion.DTOs.Usuarios;
using ObligatorioP3.LogicaNegocio.InterfacesLogicaAplicacion;
using ObligatorioP3.WebApi.Models;
using ObligatorioP3.WebApi.Services;

namespace ObligatorioP3.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILogger<LoginController> _logger;
        private readonly IAutenticarUsuarioApi<DtoUsuarioAutenticado> _autenticar;
        private readonly IJwtGenerator _jwt;

        public LoginController(IAutenticarUsuarioApi<DtoUsuarioAutenticado> autenticar, IJwtGenerator jwt, ILogger<LoginController> logger)
        {
            _logger = logger;
            _autenticar = autenticar;
            _jwt = jwt;
        }

        /// <summary>
        /// Autentica un usuario y devuelve su token JWT.
        /// </summary>
        /// <remarks>
        /// Permite iniciar sesión sin autenticación previa. Responde un objeto JSON con la propiedad token.
        /// </remarks>
        /// <param name="login">Nombre de usuario y contraseña.</param>
        /// <response code="200">Credenciales válidas; devuelve el token JWT.</response>
        /// <response code="400">Datos de solicitud inválidos.</response>
        /// <response code="401">Credenciales incorrectas.</response>
        /// <response code="500">Error interno del servidor.</response>
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Login(VMLoginApi login)
        {
            if (login is null)
                return BadRequest("Debe ingresar las credenciales.");

            try
            {
                DtoUsuarioAutenticado? usuario = _autenticar.Ejecutar(login.NombreUsuario, login.Password);
                if (usuario is null)
                    return Unauthorized(new { mensaje = "Usuario o contraseña incorrectos." });

                return Ok(new { token = _jwt.GenerateToken(usuario) });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al autenticar un usuario.");
                return StatusCode(500, new { mensaje = "Ocurrió un error inesperado. Intente nuevamente más tarde." });
            }
        }
    }
}
