
using ObligatorioP3.LogicaNegocio.Entidades;

namespace ObligatorioP3.LogicaNegocio.InterfacesRepositorio
{
    public interface IRepositorioOpcion :
        IAdd<Opcion>,
        IDelete<Opcion>,
        IGetAll<Opcion>,
        IGetById<Opcion>,
        IUpdate<Opcion>
    {
        bool ExisteOpcion(Opcion opcion);
    }
}
