namespace ObligatorioP3.LogicaAplicacion.DTOs.Lecturas
{
    public class DtoOpcionLectura
    {
        public int Id { get; set; }
        public string Texto { get; set; } = string.Empty;
    }

    public class DtoLecturaActual
    {
        public int LecturaId { get; set; }
        public int HistoriaId { get; set; }
        public int CapituloId { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Texto { get; set; } = string.Empty;
        public bool Completada { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime? FechaFinalizacion { get; set; }
        public int? FinalAlcanzadoId { get; set; }
        public List<DtoOpcionLectura> Opciones { get; set; } = new();
    }
}
