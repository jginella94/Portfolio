namespace ObligatorioP3.WebLectura.Models
{
    public class VMEstadisticasFinales
    {
        public int? HistoriaId { get; set; }
        public List<DtoHistoriaParaEstadisticasApi> Historias { get; set; } = new();
        public DtoEstadisticasFinalesApi? Estadisticas { get; set; }
        public string? MensajeError { get; set; }
    }
}
