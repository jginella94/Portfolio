using System.ComponentModel.DataAnnotations;

namespace ObligatorioP3.WebApi.Models
{
    public class VMLoginApi
    {
        [Required]
        public string NombreUsuario { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
