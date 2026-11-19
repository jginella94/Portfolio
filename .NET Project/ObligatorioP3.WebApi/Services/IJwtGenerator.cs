using ObligatorioP3.LogicaAplicacion.DTOs.Usuarios;

namespace ObligatorioP3.WebApi.Services
{
    public interface IJwtGenerator
    {
        string GenerateToken(DtoUsuarioAutenticado usuario);
    }
}
