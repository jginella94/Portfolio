namespace ObligatorioP3.WebLectura.Models
{
    public class VMDetalleLectura
    {
        public DtoLecturaActualApi Lectura { get; set; } = new();
        public string? MensajeError { get; set; }
    }
}
