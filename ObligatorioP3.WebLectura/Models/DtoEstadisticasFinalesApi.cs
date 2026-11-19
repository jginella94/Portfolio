namespace ObligatorioP3.WebLectura.Models
{
    public class DtoFrecuenciaFinalApi
    {
        public int FinalId { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public int VecesAlcanzado { get; set; }
        public bool EsMasAlcanzado { get; set; }
    }

    public class DtoEstadisticasFinalesApi
    {
        public int HistoriaId { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public List<DtoFrecuenciaFinalApi> Finales { get; set; } = new();
    }
}
