using System.ComponentModel.DataAnnotations;

namespace ObligatorioP3.WebApp.Models
{
    public class VMCapituloInicialEdicion
    {
        public int HistoriaId { get; set; }

        [Required(ErrorMessage = "Debe ingresar el título del capítulo.")]
        public string Titulo { get; set; } = string.Empty;

        [Required(ErrorMessage = "Debe ingresar el texto del capítulo.")]
        public string Texto { get; set; } = string.Empty;
    }
}
