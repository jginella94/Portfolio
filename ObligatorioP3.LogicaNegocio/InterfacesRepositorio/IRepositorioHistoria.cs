
using ObligatorioP3.LogicaNegocio.Entidades;

namespace ObligatorioP3.LogicaNegocio.InterfacesRepositorio
{
    public interface IRepositorioHistoria :
        IAdd<Historia>,
        IDelete<Historia>,
        IGetAll<Historia>,
        IGetById<Historia>,
        IUpdate<Historia>
    {
        bool ExisteHistoria(Historia historia);
    }
}
