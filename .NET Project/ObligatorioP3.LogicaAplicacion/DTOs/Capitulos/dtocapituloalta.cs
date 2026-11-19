namespace ObligatorioP3.LogicaAplicacion.DTOs.Capitulos
{
    public class DtoCapituloAlta
    {
        public int HistoriaId { get; set; }
        public int CapituloAntecedenteId { get; set; }
        public string TipoCapitulo { get; set; } = string.Empty; // "Intermedio" o "Final"
        public string Titulo { get; set; } = string.Empty;
        public string Texto { get; set; } = string.Empty;
        public string TextoOpcion { get; set; } = string.Empty;
    }
}