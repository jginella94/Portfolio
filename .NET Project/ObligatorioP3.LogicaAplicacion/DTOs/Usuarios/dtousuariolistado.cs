namespace ObligatorioP3.LogicaAplicacion.DTOs.Usuarios
{
    public record DtoUsuarioListado(
        int Id,
        string NombreCompleto,
        string Email,
        string UserName,
        string Rol       
    );
}