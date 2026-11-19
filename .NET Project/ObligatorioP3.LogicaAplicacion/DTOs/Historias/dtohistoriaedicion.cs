namespace ObligatorioP3.LogicaAplicacion.DTOs.Historias
{
    public class DtoHistoriaEdicion
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Sinopsis { get; set; } = string.Empty;
        public List<int> CategoriasIds { get; set; } = new();
        public string Estado { get; set; } = string.Empty; // "BORRADOR" o "PUBLICADA"
    }
}