namespace ObligatorioP3.WebLectura.Models
{
    public class DtoRankingHistoriaApi
    {
        public int HistoriaId { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public int CantidadLecturasCompletadas { get; set; }
    }
}
