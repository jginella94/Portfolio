using ObligatorioP3.LogicaNegocio.Entidades;
namespace ObligatorioP3.LogicaNegocio.InterfacesRepositorio
{
    public interface IRepositorioCategoria : IGetAll<Categoria>, IGetById<Categoria>
    {
    }
}
