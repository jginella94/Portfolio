using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc.Rendering;
using ObligatorioP3.LogicaAplicacion.DTOs.Categorias;
using ObligatorioP3.LogicaAplicacion.DTOs.Historias;
using ObligatorioP3.LogicaNegocio.Excepciones;
using ObligatorioP3.LogicaNegocio.InterfacesLogicaAplicacion;
using ObligatorioP3.WebApp.Filtros;
using ObligatorioP3.WebApp.Models;

namespace ObligatorioP3.WebApp.Controllers
{
    [Admin]
    public class HistoriaController : Controller
    {
        private readonly IAlta<DtoHistoriaAlta> _alta;
        private readonly IRegistrarAuditoriaHistoria _registrarAuditoria;
        private readonly ILogger<HistoriaController> _logger;
        private readonly IVer<DtoHistoriaListado> _verHistoria;
        private readonly IBorrarHistoriaAuditada _borrarHistoria;
        private readonly IListarTodos<DtoHistoriaListado> _listarHistorias;
        private readonly IListarTodos<DtoCategoria> _listarCategorias;
        private readonly IVer<DtoHistoriaEdicion> _verHistoriaEdicion;
        private readonly IEditar<DtoHistoriaEdicion> _editarHistoria;
        private readonly IVer<DtoCapituloInicialEdicion> _verCapituloInicial;
        private readonly IEditar<DtoCapituloInicialEdicion> _editarCapituloInicial;

        public HistoriaController(
            IAlta<DtoHistoriaAlta> alta,
            IVer<DtoHistoriaListado> verHistoria,
            IBorrarHistoriaAuditada borrarHistoria,
            IListarTodos<DtoHistoriaListado> listarHistorias,
            IListarTodos<DtoCategoria> listarCategorias,
            IVer<DtoHistoriaEdicion> verHistoriaEdicion,
            IEditar<DtoHistoriaEdicion> editarHistoria,
            IVer<DtoCapituloInicialEdicion> verCapituloInicial,
            IEditar<DtoCapituloInicialEdicion> editarCapituloInicial,
            IRegistrarAuditoriaHistoria registrarAuditoria,
            ILogger<HistoriaController> logger)
        {
            _alta = alta;
            _registrarAuditoria = registrarAuditoria;
            _logger = logger;
            _verHistoria = verHistoria;
            _borrarHistoria = borrarHistoria;
            _listarHistorias = listarHistorias;
            _listarCategorias = listarCategorias;
            _verHistoriaEdicion = verHistoriaEdicion;
            _editarHistoria = editarHistoria;
            _verCapituloInicial = verCapituloInicial;
            _editarCapituloInicial = editarCapituloInicial;
        }

        [HttpGet]
        public IActionResult Index()
        {
            try
            {
                return View(_listarHistorias.Ejecutar().ToList());
            }
            catch (Exception)
            {
                TempData["MensajeError"] = "No se pudo obtener el listado de historias.";
                return View(new List<DtoHistoriaListado>());
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            VMHistoriaAlta vm = new VMHistoriaAlta();
            CargarCategorias(vm);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(VMHistoriaAlta vm)
        {
            if (!ModelState.IsValid)
            {
                CargarCategorias(vm);
                return View(vm);
            }

            try
            {
                DtoHistoriaAlta dto = new DtoHistoriaAlta
                {
                    Titulo = vm.Titulo,
                    Sinopsis = vm.Sinopsis,
                    CategoriasIds = vm.CategoriasIds,
                    TituloCapituloInicial = vm.TituloCapituloInicial,
                    TextoCapituloInicial = vm.TextoCapituloInicial
                };

                int administradorId = ObtenerAdministradorId();
                int id = _alta.Ejecutar(dto);
                Auditar(id, administradorId, "CREACION_HISTORIA");
                TempData["MensajeExito"] = $"La historia #{id} fue creada correctamente en estado BORRADOR.";
                return RedirectToAction(nameof(Index));
            }
            catch (LogicaNegocioException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "No se pudo crear la historia.");
            }

            CargarCategorias(vm);
            return View(vm);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            try
            {
                var historia = _verHistoriaEdicion.Ejecutar(id);
                VMHistoriaEdicion vm = new VMHistoriaEdicion
                {
                    Id = historia.Id,
                    Titulo = historia.Titulo,
                    Sinopsis = historia.Sinopsis,
                    Estado = historia.Estado,
                    CategoriasIds = historia.CategoriasIds
                };
                CargarCategorias(vm);
                return View(vm);
            }
            catch (Exception)
            {
                TempData["MensajeError"] = "No se pudo cargar la historia para editar.";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, VMHistoriaEdicion vm)
        {
            if (id != vm.Id)
                return BadRequest();

            if (ModelState.IsValid)
            {
                try
                {
                    int administradorId = ObtenerAdministradorId();
                    string estadoAnterior = _verHistoriaEdicion.Ejecutar(id).Estado;
                    _editarHistoria.Ejecutar(id, new DtoHistoriaEdicion
                    {
                        Id = vm.Id,
                        Titulo = vm.Titulo,
                        Sinopsis = vm.Sinopsis,
                        CategoriasIds = vm.CategoriasIds,
                        Estado = vm.Estado
                    });
                    bool publicacion = !string.Equals(estadoAnterior, "PUBLICADA", StringComparison.OrdinalIgnoreCase)
                        && string.Equals(vm.Estado, "PUBLICADA", StringComparison.OrdinalIgnoreCase);
                    Auditar(id, administradorId, publicacion ? "PUBLICACION_HISTORIA" : "EDICION_HISTORIA");
                    TempData["MensajeExito"] = "Historia modificada correctamente.";
                    return RedirectToAction(nameof(Index));
                }
                catch (LogicaNegocioException ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
                catch (Exception)
                {
                    ModelState.AddModelError(string.Empty, "No se pudo modificar la historia.");
                }
            }
            CargarCategorias(vm);
            return View(vm);
        }

        [HttpGet]
        public IActionResult EditCapituloInicial(int id)
        {
            try
            {
                var capitulo = _verCapituloInicial.Ejecutar(id);
                return View(new VMCapituloInicialEdicion
                {
                    HistoriaId = capitulo.HistoriaId,
                    Titulo = capitulo.Titulo,
                    Texto = capitulo.Texto
                });
            }
            catch (Exception)
            {
                TempData["MensajeError"] = "No se pudo cargar el capítulo inicial.";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditCapituloInicial(int id, VMCapituloInicialEdicion vm)
        {
            if (id != vm.HistoriaId)
                return BadRequest();

            if (!ModelState.IsValid)
                return View(vm);

            try
            {
                int administradorId = ObtenerAdministradorId();
                _editarCapituloInicial.Ejecutar(id, new DtoCapituloInicialEdicion
                {
                    HistoriaId = vm.HistoriaId,
                    Titulo = vm.Titulo,
                    Texto = vm.Texto
                });
                Auditar(id, administradorId, "EDICION_CAPITULO_INICIAL");
                TempData["MensajeExito"] = "Capítulo inicial modificado correctamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (LogicaNegocioException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "No se pudo modificar el capítulo inicial.");
            }

            return View(vm);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            try
            {
                return View(_verHistoria.Ejecutar(id));
            }
            catch (Exception)
            {
                TempData["MensajeError"] = "No se pudo encontrar la historia para eliminar.";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                int administradorId = ObtenerAdministradorId();
                _borrarHistoria.Ejecutar(id, administradorId);
                TempData["MensajeExito"] = "La historia fue eliminada correctamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (LogicaNegocioException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "No se pudo eliminar la historia. Verifique sus relaciones e inténtelo nuevamente.");
            }

            try
            {
                return View("Delete", _verHistoria.Ejecutar(id));
            }
            catch (Exception)
            {
                TempData["MensajeError"] = "No se pudo completar la eliminación de la historia.";
                return RedirectToAction(nameof(Index));
            }
        }

        private int ObtenerAdministradorId()
        {
            int? administradorId = HttpContext.Session.GetInt32("UsuarioId");
            if (administradorId is null || administradorId <= 0)
                throw new LogicaNegocioException("La sesión no tiene un administrador válido. Vuelva a iniciar sesión.");

            return administradorId.Value;
        }

        private void Auditar(int historiaId, int administradorId, string accion)
        {
            try
            {
                _registrarAuditoria.Ejecutar(historiaId, administradorId, accion);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "No se pudo auditar la acción {Accion} de la historia {HistoriaId}", accion, historiaId);
                TempData["MensajeError"] = "La operación se guardó, pero no pudo registrarse su auditoría. Informe al administrador.";
            }
        }

        private void CargarCategorias(VMHistoriaAlta vm)
        {
            try
            {
                vm.CategoriasDisponibles = ObtenerCategorias();
            }
            catch (Exception)
            {
                vm.CategoriasDisponibles = new List<SelectListItem>();
                ModelState.AddModelError(string.Empty, "No se pudieron cargar las categorías.");
            }
        }

        private void CargarCategorias(VMHistoriaEdicion vm)
        {
            try
            {
                vm.CategoriasDisponibles = ObtenerCategorias();
            }
            catch (Exception)
            {
                vm.CategoriasDisponibles = new List<SelectListItem>();
                ModelState.AddModelError(string.Empty, "No se pudieron cargar las categorías.");
            }
        }

        private List<SelectListItem> ObtenerCategorias()
        {
            return _listarCategorias.Ejecutar()
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Nombre
                })
                .OrderBy(c => c.Text)
                .ToList();
        }
    }
}
