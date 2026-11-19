using Microsoft.AspNetCore.Mvc.Rendering;
using ObligatorioP3.LogicaAplicacion.DTOs.AuditoriasHistorias;

namespace ObligatorioP3.WebApp.Models
{
    public class VMAuditoriaConsulta
    {
        public int? HistoriaId { get; set; }
        public int? AdministradorId { get; set; }
        public List<SelectListItem> HistoriasDisponibles { get; set; } = new();
        public List<SelectListItem> AdministradoresDisponibles { get; set; } = new();
        public List<DtoAuditoriaHistoria> Auditorias { get; set; } = new();
    }
}
