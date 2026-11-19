using ObligatorioP3.LogicaNegocio.Entidades;

namespace ObligatorioP3.LogicaNegocio.InterfacesRepositorio
{
    public interface IRepositorioRankingLectores
    {
        IEnumerable<(Lector Lector, int Cantidad)> ObtenerRanking();
    }
}
