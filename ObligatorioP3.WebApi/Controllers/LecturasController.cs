using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ObligatorioP3.Infraestructura.AccesoDatos.EF.Excepciones;
using ObligatorioP3.LogicaAplicacion.DTOs.Lecturas;
using ObligatorioP3.LogicaNegocio.Excepciones;
using ObligatorioP3.LogicaNegocio.InterfacesLogicaAplicacion;
using System.Security.Claims;

namespace ObligatorioP3.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Usuario")]
    public class LecturasController : ControllerBase
    {
        private readonly ILogger<LecturasController> _logger;
        private readonly IIniciarLectura<DtoLecturaActual> _iniciar;
        private readonly IConsultarLectura<DtoLecturaActual> _consultar;
        private readonly IElegirOpcionLectura<DtoLecturaActual> _elegir;
        private readonly IConsultarHistorialLecturas<DtoLecturaHistorial> _historial;

        public LecturasController(
            IIniciarLectura<DtoLecturaActual> iniciar,
            IConsultarLectura<DtoLecturaActual> consultar,
            IElegirOpcionLectura<DtoLecturaActual> elegir,
            IConsultarHistorialLecturas<DtoLecturaHistorial> historial, ILogger<LecturasController> logger)
        {
            _logger = logger;
            _iniciar = iniciar;
            _consultar = consultar;
            _elegir = elegir;
            _historial = historial;
        }

        /// <summary>
        /// Inicia una lectura de una historia publicada.
        /// </summary>
        /// <remarks>
        /// Registra una lectura nueva para el usuario autenticado. Requiere rol Usuario.
        /// </remarks>
        /// <param name="solicitud">Identificador de la historia que se quiere leer.</param>
        /// <response code="201">Lectura creada; devuelve el capítulo inicial.</response>
        /// <response code="400">Datos incorrectos o regla de negocio incumplida.</response>
        /// <response code="401">No autenticado.</response>
        /// <response code="403">Acceso denegado.</response>
        /// <response code="404">Historia inexistente.</response>
        /// <response code="500">Error interno del servidor.</response>
        [HttpPost]
        [ProducesResponseType(typeof(DtoLecturaActual), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Iniciar([FromBody] DtoIniciarLectura solicitud)
        {
            if (solicitud is null || solicitud.HistoriaId <= 0)
                return BadRequest("La historia no es válida.");

            int? usuarioId = ObtenerUsuarioId();
            if (!usuarioId.HasValue)
                return Unauthorized();

            try
            {
                DtoLecturaActual lectura = _iniciar.Ejecutar(solicitud.HistoriaId, usuarioId.Value);
                return CreatedAtAction(nameof(Obtener), new { id = lectura.LecturaId }, lectura);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (LogicaNegocioException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al iniciar una lectura.");
                return StatusCode(500, new { mensaje = "Ocurrió un error inesperado. Intente nuevamente más tarde." });
            }
        }

        /// <summary>
        /// Obtiene el historial de lecturas del usuario autenticado.
        /// </summary>
        /// <remarks>
        /// Filtra por el mes y año de inicio de lectura en UTC. Requiere rol Usuario.
        /// </remarks>
        /// <param name="mes">Mes del historial, entre 1 y 12.</param>
        /// <param name="anio">Año del historial, entre 1 y 9998.</param>
        /// <response code="200">Historial obtenido.</response>
        /// <response code="400">Mes o año inválido.</response>
        /// <response code="401">No autenticado.</response>
        /// <response code="403">Acceso denegado.</response>
        /// <response code="500">Error interno del servidor.</response>
        [HttpGet("historial")]
        [ProducesResponseType(typeof(IEnumerable<DtoLecturaHistorial>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Historial([FromQuery] int mes, [FromQuery] int anio)
        {
            int? usuarioId = ObtenerUsuarioId();
            if (!usuarioId.HasValue)
                return Unauthorized();

            if (mes < 1 || mes > 12 || anio < 1 || anio > 9998)
                return BadRequest("El mes o el año no son válidos.");

            try
            {
                return Ok(_historial.Ejecutar(usuarioId.Value, mes, anio));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al consultar el historial de lecturas.");
                return StatusCode(500, new { mensaje = "Ocurrió un error inesperado. Intente nuevamente más tarde." });
            }
        }

        /// <summary>
        /// Consulta el capítulo actual de una lectura propia.
        /// </summary>
        /// <remarks>
        /// Requiere un JWT con rol Usuario y que la lectura pertenezca al lector.
        /// </remarks>
        /// <param name="id">Identificador de la lectura.</param>
        /// <response code="200">Estado actual de la lectura.</response>
        /// <response code="401">No autenticado.</response>
        /// <response code="403">Acceso denegado.</response>
        /// <response code="404">Lectura inexistente o no perteneciente al lector.</response>
        /// <response code="500">Error interno del servidor.</response>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(DtoLecturaActual), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Obtener(int id)
        {
            int? usuarioId = ObtenerUsuarioId();
            if (!usuarioId.HasValue)
                return Unauthorized();

            try
            {
                return Ok(_consultar.Ejecutar(id, usuarioId.Value));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al consultar una lectura.");
                return StatusCode(500, new { mensaje = "Ocurrió un error inesperado. Intente nuevamente más tarde." });
            }
        }

        /// <summary>
        /// Avanza la lectura a partir de una opción del capítulo actual.
        /// </summary>
        /// <remarks>
        /// La opción debe pertenecer al capítulo actual; no se puede continuar una lectura completada. Requiere rol Usuario.
        /// </remarks>
        /// <param name="id">Identificador de la lectura.</param>
        /// <param name="opcionId">Identificador de la opción seleccionada.</param>
        /// <response code="200">Nuevo capítulo o final alcanzado.</response>
        /// <response code="400">Opción inválida o lectura ya completada.</response>
        /// <response code="401">No autenticado.</response>
        /// <response code="403">Acceso denegado.</response>
        /// <response code="404">Lectura u opción inexistente.</response>
        /// <response code="500">Error interno del servidor.</response>
        [HttpPost("{id:int}/opciones/{opcionId:int}")]
        [ProducesResponseType(typeof(DtoLecturaActual), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Elegir(int id, int opcionId)
        {
            int? usuarioId = ObtenerUsuarioId();
            if (!usuarioId.HasValue)
                return Unauthorized();

            try
            {
                return Ok(_elegir.Ejecutar(id, opcionId, usuarioId.Value));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (LogicaNegocioException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al elegir una opción de lectura.");
                return StatusCode(500, new { mensaje = "Ocurrió un error inesperado. Intente nuevamente más tarde." });
            }
        }

        private int? ObtenerUsuarioId()
        {
            string? valor = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.TryParse(valor, out int id) && id > 0 ? id : null;
        }
    }
}
