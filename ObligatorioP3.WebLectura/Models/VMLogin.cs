using System.ComponentModel.DataAnnotations;

namespace ObligatorioP3.WebLectura.Models
{
    public class VMLogin
    {
        [Required(ErrorMessage = "Ingrese su nombre de usuario.")]
        [Display(Name = "Nombre de usuario")]
        public string NombreUsuario { get; set; } = string.Empty;

        [Required(ErrorMessage = "Ingrese su contraseña.")]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; } = string.Empty;
    }
}
