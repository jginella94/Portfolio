namespace ObligatorioP3.LogicaAplicacion.DTOs.Usuarios
{
    public class DtoUsuarioAlta
    {
        public string NombreCompleto { get; set; } = string.Empty;
        public string NombreUsuario { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Rol { get; set; } = string.Empty;
    }
}
