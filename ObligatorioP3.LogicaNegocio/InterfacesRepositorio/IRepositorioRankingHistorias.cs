using ObligatorioP3.LogicaNegocio.Entidades;

namespace ObligatorioP3.LogicaNegocio.InterfacesRepositorio
{
    public interface IRepositorioRankingHistorias
    {
        IEnumerable<(Historia Historia, int Cantidad)> ObtenerRanking();
    }
}
