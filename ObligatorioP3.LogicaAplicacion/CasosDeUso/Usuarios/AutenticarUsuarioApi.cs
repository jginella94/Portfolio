using ObligatorioP3.LogicaAplicacion.DTOs.Usuarios;
using ObligatorioP3.LogicaNegocio.InterfacesLogicaAplicacion;
using ObligatorioP3.LogicaNegocio.Entidades;
using ObligatorioP3.LogicaNegocio.InterfacesRepositorio;

namespace ObligatorioP3.LogicaAplicacion.CasosDeUso.Usuarios
{
    public class AutenticarUsuarioApi : IAutenticarUsuarioApi<DtoUsuarioAutenticado>
    {
        private readonly IRepositorioUsuario _usuarios;

        public AutenticarUsuarioApi(IRepositorioUsuario usuarios)
        {
            _usuarios = usuarios;
        }

        public DtoUsuarioAutenticado? Ejecutar(string nombreUsuario, string password)
        {
            if (string.IsNullOrWhiteSpace(nombreUsuario) || string.IsNullOrEmpty(password))
                return null;

            Usuario? usuario = _usuarios.BuscarPorNombreUsuario(nombreUsuario.Trim());
            if (usuario is null || !usuario.Password.Verificar(password))
                return null;

            string rol = usuario is Administrador ? "Administrador" : usuario is Lector ? "Usuario" : string.Empty;
            if (string.IsNullOrEmpty(rol))
                return null;

            return new DtoUsuarioAutenticado(
                usuario.Id,
                usuario.NombreCompleto.Value,
                usuario.Email.Value,
                rol);
        }
    }
}
