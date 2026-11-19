using ObligatorioP3.LogicaNegocio.Entidades;

namespace ObligatorioP3.LogicaNegocio.InterfacesRepositorio
{
    public interface IRepositorioUsuario :
        IAdd<Usuario>,
        IDelete<Usuario>,
        IGetAll<Usuario>,
        IGetById<Usuario>,
        IUpdate<Usuario>
    {
        bool ExisteUsuario(Usuario usuario);
        Usuario? BuscarPorNombreUsuario(string nombreUsuario);
    }
}
