namespace ObligatorioP3.WebLectura.Models
{
    public class VMRankingLectores
    {
        public List<DtoRankingLectorApi> Lectores { get; set; } = new();
        public string? MensajeError { get; set; }
    }
}
