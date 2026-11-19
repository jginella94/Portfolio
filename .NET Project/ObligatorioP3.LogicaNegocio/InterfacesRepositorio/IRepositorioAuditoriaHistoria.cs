using ObligatorioP3.LogicaNegocio.Entidades;

namespace ObligatorioP3.LogicaNegocio.InterfacesRepositorio
{
    public interface IRepositorioAuditoriaHistoria : IAdd<AuditoriaHistoria>
    {
        IEnumerable<AuditoriaHistoria> ObtenerPorFiltros(int? historiaId, int? administradorId);
    }
}
