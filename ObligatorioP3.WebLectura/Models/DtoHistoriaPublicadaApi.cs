namespace ObligatorioP3.WebLectura.Models
{
    public class DtoHistoriaPublicadaApi
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Sinopsis { get; set; } = string.Empty;
        public List<string> Categorias { get; set; } = new();
    }
}
