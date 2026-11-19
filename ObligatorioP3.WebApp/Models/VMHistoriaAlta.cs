using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace ObligatorioP3.WebApp.Models
{
    public class VMHistoriaAlta
    {
        [Required(ErrorMessage = "Debe ingresar el título de la historia.")]
        public string Titulo { get; set; } = string.Empty;

        [Required(ErrorMessage = "Debe ingresar la sinopsis.")]
        public string Sinopsis { get; set; } = string.Empty;

        [Required(ErrorMessage = "Debe seleccionar al menos una categoría.")]
        [MinLength(1, ErrorMessage = "Debe seleccionar al menos una categoría.")]
        public List<int> CategoriasIds { get; set; } = new();

        [Required(ErrorMessage = "Debe ingresar el título del capítulo inicial.")]
        public string TituloCapituloInicial { get; set; } = string.Empty;

        [Required(ErrorMessage = "Debe ingresar el texto del capítulo inicial.")]
        public string TextoCapituloInicial { get; set; } = string.Empty;

        public List<SelectListItem> CategoriasDisponibles { get; set; } = new();
    }
}
