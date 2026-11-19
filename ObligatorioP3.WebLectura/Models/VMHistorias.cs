using Microsoft.AspNetCore.Mvc.Rendering;

namespace ObligatorioP3.WebLectura.Models
{
    public class VMHistorias
    {
        public int? CategoriaId { get; set; }
        public List<SelectListItem> CategoriasDisponibles { get; set; } = new();
        public List<DtoHistoriaPublicadaApi> Historias { get; set; } = new();
        public string? MensajeError { get; set; }
    }
}
