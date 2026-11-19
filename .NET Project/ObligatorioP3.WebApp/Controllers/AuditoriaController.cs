using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ObligatorioP3.LogicaAplicacion.DTOs.AuditoriasHistorias;
using ObligatorioP3.LogicaAplicacion.DTOs.Historias;
using ObligatorioP3.LogicaNegocio.InterfacesLogicaAplicacion;
using ObligatorioP3.WebApp.Filtros;
using ObligatorioP3.WebApp.Models;

namespace ObligatorioP3.WebApp.Controllers
{
    [Admin]
    public class AuditoriaController : Controller
    {
        private readonly IConsultarAuditorias<DtoAuditoriaHistoria> _consultarAuditorias;
        private readonly IListarTodos<DtoHistoriaListado> _listarHistorias;
        private readonly IListarTodos<DtoAdministradorFiltroAuditoria> _listarAdministradores;

        public AuditoriaController(
            IConsultarAuditorias<DtoAuditoriaHistoria> consultarAuditorias,
            IListarTodos<DtoHistoriaListado> listarHistorias,
            IListarTodos<DtoAdministradorFiltroAuditoria> listarAdministradores)
        {
            _consultarAuditorias = consultarAuditorias;
            _listarHistorias = listarHistorias;
            _listarAdministradores = listarAdministradores;
        }

        [HttpGet]
        public IActionResult Index(int? historiaId, int? administradorId)
        {
            var vm = new VMAuditoriaConsulta
            {
                HistoriaId = historiaId,
                AdministradorId = administradorId
            };

            try
            {
                vm.HistoriasDisponibles = _listarHistorias.Ejecutar()
                    .OrderBy(h => h.Titulo)
                    .Select(h => new SelectListItem
                    {
                        Value = h.Id.ToString(),
                        Text = h.Titulo
                    }).ToList();

                vm.AdministradoresDisponibles = _listarAdministradores.Ejecutar()
                    .Select(a => new SelectListItem
                    {
                        Value = a.Id.ToString(),
                        Text = a.NombreCompleto
                    }).ToList();

                vm.Auditorias = _consultarAuditorias.Ejecutar(historiaId, administradorId).ToList();
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "No se pudieron consultar las auditorías.");
            }

            return View(vm);
        }
    }
}
