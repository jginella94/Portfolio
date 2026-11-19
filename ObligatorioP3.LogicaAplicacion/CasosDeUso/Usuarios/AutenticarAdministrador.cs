using ObligatorioP3.LogicaNegocio.Entidades;
using ObligatorioP3.LogicaNegocio.InterfacesLogicaAplicacion;
using ObligatorioP3.LogicaNegocio.InterfacesRepositorio;

namespace ObligatorioP3.LogicaAplicacion.CasosDeUso.Usuarios
{
    public class AutenticarAdministrador : IAutenticar
    {
        private readonly IRepositorioUsuario _repositorioUsuario;

        public AutenticarAdministrador(IRepositorioUsuario repositorioUsuario)
        {
            _repositorioUsuario = repositorioUsuario;
        }

        public int? Ejecutar(string nombreUsuario, string password)
        {
            if (string.IsNullOrWhiteSpace(nombreUsuario) || string.IsNullOrEmpty(password))
                return null;

            Usuario? usuario = _repositorioUsuario.BuscarPorNombreUsuario(nombreUsuario.Trim());

            if (usuario is not Administrador || !usuario.Password.Verificar(password))
                return null;

            return usuario.Id;
        }
    }
}
