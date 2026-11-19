using System.ComponentModel.DataAnnotations;

namespace ObligatorioP3.WebApp.Models
{
    public class VMUsuario
    {
        [Required(ErrorMessage = "Debe ingresar el nombre completo.")]
        public string NombreCompleto { get; set; } = string.Empty;

        [Required(ErrorMessage = "Debe ingresar el nombre de usuario.")]
        public string NombreUsuario { get; set; } = string.Empty;

        [Required(ErrorMessage = "Debe ingresar el email.")]
        [EmailAddress(ErrorMessage = "Debe ingresar un email válido.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Debe ingresar la contraseña.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Debe seleccionar un rol.")]
        public string Rol { get; set; } = string.Empty;
    }
}
