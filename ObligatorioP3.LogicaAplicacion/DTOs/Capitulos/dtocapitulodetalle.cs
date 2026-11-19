namespace ObligatorioP3.LogicaAplicacion.DTOs.Capitulos
{
    public class DtoCapituloDetalle
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Texto { get; set; } = string.Empty;
        public string TipoCapitulo { get; set; } = string.Empty;
        public List<DtoOpcion> Opciones { get; set; } = new();
        public int? ContadorVecesAlcanzado { get; set; }
    }

    public class DtoOpcion
    {
        public int Id { get; set; }
        public string Texto { get; set; } = string.Empty;
        public int CapituloDestinoId { get; set; }
    }
}