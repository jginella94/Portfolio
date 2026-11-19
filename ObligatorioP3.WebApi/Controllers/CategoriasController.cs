using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ObligatorioP3.LogicaAplicacion.DTOs.Categorias;
using ObligatorioP3.LogicaNegocio.InterfacesLogicaAplicacion;

namespace ObligatorioP3.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Usuario")]
    public class CategoriasController : ControllerBase
    {
        private readonly ILogger<CategoriasController> _logger;
        private readonly IListarTodos<DtoCategoria> _listarCategorias;

        public CategoriasController(IListarTodos<DtoCategoria> listarCategorias, ILogger<CategoriasController> logger)
        {
            _logger = logger;
            _listarCategorias = listarCategorias;
        }

        /// <summary>
        /// Lista las categorías disponibles para los lectores.
        /// </summary>
        /// <remarks>
        /// Requiere un JWT con rol Usuario.
        /// </remarks>
        /// <response code="200">Categorías obtenidas correctamente.</response>
        /// <response code="401">No autenticado.</response>
        /// <response code="403">El rol no tiene permiso.</response>
        /// <response code="500">Error interno del servidor.</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<DtoCategoria>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Get()
        {
            try
            {
                return Ok(_listarCategorias.Ejecutar());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al listar categorías.");
                return StatusCode(500, new { mensaje = "Ocurrió un error inesperado. Intente nuevamente más tarde." });
            }
        }
    }
}
