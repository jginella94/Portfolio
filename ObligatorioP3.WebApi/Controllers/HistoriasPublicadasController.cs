using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ObligatorioP3.LogicaAplicacion.DTOs.Historias;
using ObligatorioP3.LogicaNegocio.InterfacesLogicaAplicacion;

namespace ObligatorioP3.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Usuario")]
    public class HistoriasPublicadasController : ControllerBase
    {
        private readonly ILogger<HistoriasPublicadasController> _logger;
        private readonly IListarHistoriasPublicadas<DtoHistoriaPublicada> _listar;

        public HistoriasPublicadasController(IListarHistoriasPublicadas<DtoHistoriaPublicada> listar, ILogger<HistoriasPublicadasController> logger)
        {
            _logger = logger;
            _listar = listar;
        }

        /// <summary>
        /// Lista historias publicadas, opcionalmente filtradas por categoría.
        /// </summary>
        /// <remarks>
        /// Requiere un JWT con rol Usuario.
        /// </remarks>
        /// <param name="categoriaId">Identificador opcional de la categoría.</param>
        /// <response code="200">Historias publicadas obtenidas.</response>
        /// <response code="400">El filtro de categoría es inválido.</response>
        /// <response code="401">No autenticado.</response>
        /// <response code="403">Acceso denegado.</response>
        /// <response code="500">Error interno del servidor.</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<DtoHistoriaPublicada>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Get([FromQuery] int? categoriaId)
        {
            if (categoriaId.HasValue && categoriaId.Value <= 0)
                return BadRequest("La categoría no es válida.");

            try
            {
                return Ok(_listar.Ejecutar(categoriaId));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al listar historias publicadas.");
                return StatusCode(500, new { mensaje = "Ocurrió un error inesperado. Intente nuevamente más tarde." });
            }
        }
    }
}
