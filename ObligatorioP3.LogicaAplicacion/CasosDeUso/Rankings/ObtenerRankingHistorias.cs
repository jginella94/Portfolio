using ObligatorioP3.LogicaAplicacion.DTOs.Rankings;
using ObligatorioP3.LogicaNegocio.InterfacesLogicaAplicacion;
using ObligatorioP3.LogicaNegocio.InterfacesRepositorio;

namespace ObligatorioP3.LogicaAplicacion.CasosDeUso.Rankings
{
    public class ObtenerRankingHistorias : IObtenerRankingHistorias<DtoRankingHistoria>
    {
        private readonly IRepositorioRankingHistorias _ranking;

        public ObtenerRankingHistorias(IRepositorioRankingHistorias ranking)
        {
            _ranking = ranking;
        }

        public IEnumerable<DtoRankingHistoria> Ejecutar()
        {
            return _ranking.ObtenerRanking()
                .Select(r => new DtoRankingHistoria(
                    r.Historia.Id,
                    r.Historia.Titulo.Value,
                    r.Cantidad))
                .ToList();
        }
    }
}
