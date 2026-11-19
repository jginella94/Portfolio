namespace ObligatorioP3.WebLectura.Models
{
    public class DtoLecturaHistorialApi
    {
        public int LecturaId { get; set; }
        public int HistoriaId { get; set; }
        public string TituloHistoria { get; set; } = string.Empty;
        public DateTime FechaInicio { get; set; }
        public DateTime? FechaFinalizacion { get; set; }
        public bool Completada { get; set; }
        public int? FinalAlcanzadoId { get; set; }
        public string? TituloFinal { get; set; }
    }
}
