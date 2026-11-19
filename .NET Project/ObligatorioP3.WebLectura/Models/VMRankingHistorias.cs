namespace ObligatorioP3.WebLectura.Models
{
    public class VMRankingHistorias
    {
        public List<DtoRankingHistoriaApi> Historias { get; set; } = new();
        public string? MensajeError { get; set; }
    }
}
