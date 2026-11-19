using ObligatorioP3.LogicaNegocio.Entidades;

namespace ObligatorioP3.LogicaNegocio.InterfacesRepositorio
{
    public interface IRepositorioHistoriasPublicadas
    {
        IEnumerable<Historia> ObtenerPublicadas(int? categoriaId);
    }
}
