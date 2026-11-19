namespace ObligatorioP3.WebLectura.Models
{
    public class VMHistorialLecturas
    {
        public int Mes { get; set; }
        public int Anio { get; set; }
        public List<DtoLecturaHistorialApi> Lecturas { get; set; } = new();
        public string? MensajeError { get; set; }
    }
}
