using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace ObligatorioP3.WebApp.Models
{
    public class VMHistoriaEdicion
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Debe ingresar el título de la historia.")]
        public string Titulo { get; set; } = string.Empty;

        [Required(ErrorMessage = "Debe ingresar la sinopsis.")]
        public string Sinopsis { get; set; } = string.Empty;

        [MinLength(1, ErrorMessage = "Debe seleccionar al menos una categoría.")]
        public List<int> CategoriasIds { get; set; } = new();

        [Required(ErrorMessage = "Debe seleccionar un estado.")]
        public string Estado { get; set; } = "BORRADOR";

        public List<SelectListItem> CategoriasDisponibles { get; set; } = new();
    }
}
