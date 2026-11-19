namespace ObligatorioP3.WebLectura.Models
{
    public class DtoRankingLectorApi
    {
        public int LectorId { get; set; }
        public string NombreUsuario { get; set; } = string.Empty;
        public int CantidadHistoriasCompletadas { get; set; }
    }
}
