using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc.Rendering;
using ObligatorioP3.LogicaAplicacion.DTOs.Capitulos;
using ObligatorioP3.LogicaNegocio.Excepciones;
using ObligatorioP3.LogicaNegocio.InterfacesLogicaAplicacion;
using ObligatorioP3.WebApp.Filtros;
using ObligatorioP3.WebApp.Models;

namespace ObligatorioP3.WebApp.Controllers
{
    [Admin]
    public class CapituloController : Controller
    {
        private readonly IAlta<DtoCapituloAgregar> _altaCapitulo;
        private readonly IRegistrarAuditoriaHistoria _registrarAuditoria;
        private readonly ILogger<CapituloController> _logger;
        private readonly IVer<DtoCapitulosParaAlta> _verCapitulos;

        public CapituloController(
            IAlta<DtoCapituloAgregar> altaCapitulo,
            IVer<DtoCapitulosParaAlta> verCapitulos,
            IRegistrarAuditoriaHistoria registrarAuditoria,
            ILogger<CapituloController> logger)
        {
            _altaCapitulo = altaCapitulo;
            _verCapitulos = verCapitulos;
            _registrarAuditoria = registrarAuditoria;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Create(int historiaId)
        {
            VMCapituloAgregar vm = new VMCapituloAgregar { HistoriaId = historiaId };

            if (!CargarAntecedentes(vm))
                return RedirectToAction("Index", "Historia");

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(VMCapituloAgregar vm)
        {
            if (vm.Tipo != "Intermedio" && vm.Tipo != "Final")
                ModelState.AddModelError(nameof(vm.Tipo), "Seleccione un tipo de capítulo válido.");

            if (ModelState.IsValid)
            {
                try
                {
                    int administradorId = ObtenerAdministradorId();
                    int id = _altaCapitulo.Ejecutar(new DtoCapituloAgregar
                    {
                        HistoriaId = vm.HistoriaId,
                        CapituloAntecedenteId = vm.CapituloAntecedenteId,
                        Tipo = vm.Tipo,
                        Titulo = vm.Titulo,
                        Texto = vm.Texto,
                        TextoOpcion = vm.TextoOpcion
                    });
                    Auditar(vm.HistoriaId, administradorId, vm.Tipo == "Final" ? "CREACION_CAPITULO_FINAL" : "CREACION_CAPITULO_INTERMEDIO");

                    TempData["MensajeExito"] = $"El capítulo #{id} fue agregado correctamente.";
                    return RedirectToAction("Index", "Historia");
                }
                catch (LogicaNegocioException ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
                catch (Exception)
                {
                    ModelState.AddModelError(string.Empty, "No se pudo agregar el capítulo. Verifique los datos e inténtelo nuevamente.");
                }
            }

            if (!CargarAntecedentes(vm))
                return RedirectToAction("Index", "Historia");

            return View(vm);
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
                TempData["MensajeError"] = "El capítulo se guardó, pero no pudo registrarse su auditoría. Informe al administrador.";
            }
        }

        private bool CargarAntecedentes(VMCapituloAgregar vm)
        {
            try
            {
                DtoCapitulosParaAlta datos = _verCapitulos.Ejecutar(vm.HistoriaId);
                vm.TituloHistoria = datos.TituloHistoria;
                vm.Antecedentes = datos.CapitulosIntermedios
                    .Select(c => new SelectListItem
                    {
                        Value = c.Id.ToString(),
                        Text = $"#{c.Id} - {c.Titulo}"
                    })
                    .ToList();

                if (datos.Estado == "PUBLICADA")
                {
                    TempData["MensajeError"] = "No se pueden agregar capítulos a una historia publicada.";
                    return false;
                }

                return true;
            }
            catch (Exception)
            {
                TempData["MensajeError"] = "No se pudo recuperar la historia o sus capítulos.";
                return false;
            }
        }
    }
}
