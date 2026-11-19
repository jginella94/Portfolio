using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ObligatorioP3.LogicaAplicacion.DTOs.Rankings;
using ObligatorioP3.LogicaNegocio.InterfacesLogicaAplicacion;

namespace ObligatorioP3.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Administrador")]
    public class RankingLectoresController : ControllerBase
    {
        private readonly ILogger<RankingLectoresController> _logger;
        private readonly IObtenerRankingLectores<DtoRankingLector> _ranking;

        public RankingLectoresController(IObtenerRankingLectores<DtoRankingLector> ranking, ILogger<RankingLectoresController> logger)
        {
            _logger = logger;
            _ranking = ranking;
        }

        /// <summary>
        /// Obtiene el ranking de lectores por historias distintas completadas.
        /// </summary>
        /// <remarks>
        /// Cada historia cuenta una sola vez por lector. Requiere rol Administrador.
        /// </remarks>
        /// <response code="200">Ranking obtenido.</response>
        /// <response code="401">No autenticado.</response>
        /// <response code="403">Acceso denegado.</response>
        /// <response code="500">Error interno del servidor.</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<DtoRankingLector>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Get()
        {
            try
            {
                return Ok(_ranking.Ejecutar());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al consultar el ranking de lectores.");
                return StatusCode(500, new { mensaje = "Ocurrió un error inesperado. Intente nuevamente más tarde." });
            }
        }
    }
}
