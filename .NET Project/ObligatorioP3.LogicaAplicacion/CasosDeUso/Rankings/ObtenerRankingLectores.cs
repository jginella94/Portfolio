using ObligatorioP3.LogicaAplicacion.DTOs.Rankings;
using ObligatorioP3.LogicaNegocio.InterfacesLogicaAplicacion;
using ObligatorioP3.LogicaNegocio.InterfacesRepositorio;

namespace ObligatorioP3.LogicaAplicacion.CasosDeUso.Rankings
{
    public class ObtenerRankingLectores : IObtenerRankingLectores<DtoRankingLector>
    {
        private readonly IRepositorioRankingLectores _ranking;

        public ObtenerRankingLectores(IRepositorioRankingLectores ranking)
        {
            _ranking = ranking;
        }

        public IEnumerable<DtoRankingLector> Ejecutar()
        {
            return _ranking.ObtenerRanking()
                .Select(r => new DtoRankingLector(
                    r.Lector.Id,
                    r.Lector.NombreUsuario.Value,
                    r.Cantidad))
                .ToList();
        }
    }
}
