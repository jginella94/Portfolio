using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace ObligatorioP3.WebApp.Models
{
    public class VMCapituloAgregar
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int HistoriaId { get; set; }

        [Required(ErrorMessage = "Seleccione el capítulo antecedente.")]
        [Range(1, int.MaxValue, ErrorMessage = "Seleccione el capítulo antecedente.")]
        [Display(Name = "Capítulo antecedente")]
        public int CapituloAntecedenteId { get; set; }

        [Required(ErrorMessage = "Seleccione un tipo de capítulo.")]
        [Display(Name = "Tipo de capítulo")]
        public string Tipo { get; set; } = string.Empty;

        [Required(ErrorMessage = "Ingrese el título del capítulo.")]
        [Display(Name = "Título del capítulo")]
        public string Titulo { get; set; } = string.Empty;

        [Required(ErrorMessage = "Ingrese el texto del capítulo.")]
        [Display(Name = "Texto del capítulo")]
        public string Texto { get; set; } = string.Empty;

        [Required(ErrorMessage = "Ingrese el texto de la opción.")]
        [Display(Name = "Texto de la opción")]
        public string TextoOpcion { get; set; } = string.Empty;

        public string TituloHistoria { get; set; } = string.Empty;
        public List<SelectListItem> Antecedentes { get; set; } = new();
    }
}
