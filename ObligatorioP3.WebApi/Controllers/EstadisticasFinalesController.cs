using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ObligatorioP3.LogicaAplicacion.DTOs.Rankings;
using ObligatorioP3.LogicaNegocio.InterfacesLogicaAplicacion;

namespace ObligatorioP3.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Administrador")]
    public class EstadisticasFinalesController : ControllerBase
    {
        private readonly ILogger<EstadisticasFinalesController> _logger;
        private readonly IConsultarEstadisticasFinales<DtoEstadisticasFinales> _consulta;
        private readonly IListarHistoriasParaEstadisticas<DtoHistoriaParaEstadisticas> _listar;

        public EstadisticasFinalesController(
            IConsultarEstadisticasFinales<DtoEstadisticasFinales> consulta,
            IListarHistoriasParaEstadisticas<DtoHistoriaParaEstadisticas> listar, ILogger<EstadisticasFinalesController> logger)
        {
            _logger = logger;
            _consulta = consulta;
            _listar = listar;
        }

        /// <summary>
        /// Lista las historias disponibles para consultar estadísticas de finales.
        /// </summary>
        /// <remarks>
        /// Requiere un JWT con rol Administrador.
        /// </remarks>
        /// <response code="200">Historias disponibles.</response>
        /// <response code="401">No autenticado.</response>
        /// <response code="403">Acceso denegado.</response>
        /// <response code="500">Error interno del servidor.</response>
        [HttpGet("historias")]
        [ProducesResponseType(typeof(IEnumerable<DtoHistoriaParaEstadisticas>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Historias()
        {
            try
            {
                return Ok(_listar.Ejecutar());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al listar historias para estadísticas.");
                return StatusCode(500, new { mensaje = "Ocurrió un error inesperado. Intente nuevamente más tarde." });
            }
        }

        /// <summary>
        /// Obtiene la frecuencia de los finales de una historia.
        /// </summary>
        /// <remarks>
        /// Incluye finales con cero llegadas y marca los máximos positivos. Requiere rol Administrador.
        /// </remarks>
        /// <param name="historiaId">Identificador de la historia a consultar.</param>
        /// <response code="200">Estadísticas obtenidas.</response>
        /// <response code="400">El identificador es inválido.</response>
        /// <response code="401">No autenticado.</response>
        /// <response code="403">Acceso denegado.</response>
        /// <response code="404">La historia no existe.</response>
        /// <response code="500">Error interno del servidor.</response>
        [HttpGet("{historiaId:int}")]
        [ProducesResponseType(typeof(DtoEstadisticasFinales), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Get(int historiaId)
        {
            if (historiaId <= 0)
                return BadRequest("El identificador de la historia debe ser mayor a cero.");

            try
            {
                var resultado = _consulta.Ejecutar(historiaId);
                if (resultado is null)
                    return NotFound("La historia no existe.");

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al consultar estadísticas de finales.");
                return StatusCode(500, new { mensaje = "Ocurrió un error inesperado. Intente nuevamente más tarde." });
            }
        }
    }
}
