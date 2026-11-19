
using ObligatorioP3.LogicaNegocio.Entidades;

namespace ObligatorioP3.LogicaNegocio.InterfacesRepositorio
{
    public interface IRepositorioCapitulo :
        IAdd<Capitulo>,
        IDelete<Capitulo>,
        IGetAll<Capitulo>,
        IGetById<Capitulo>,
        IUpdate<Capitulo>
    {
        bool ExisteCapitulo(Capitulo capitulo);
    }
}
