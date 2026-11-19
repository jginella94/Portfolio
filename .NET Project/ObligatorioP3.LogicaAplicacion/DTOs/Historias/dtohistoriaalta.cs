namespace ObligatorioP3.LogicaAplicacion.DTOs.Historias
{
    public class DtoHistoriaAlta
    {
        public string Titulo { get; set; } = string.Empty;
        public string Sinopsis { get; set; } = string.Empty;
        public List<int> CategoriasIds { get; set; } = new();
        public string TituloCapituloInicial { get; set; } = string.Empty;
        public string TextoCapituloInicial { get; set; } = string.Empty;
    }
}