using ObligatorioP3.LogicaAplicacion.DTOs.Rankings;
using ObligatorioP3.LogicaNegocio.InterfacesLogicaAplicacion;
using ObligatorioP3.LogicaNegocio.InterfacesRepositorio;

namespace ObligatorioP3.LogicaAplicacion.CasosDeUso.Rankings
{
    public class ListarHistoriasParaEstadisticas : IListarHistoriasParaEstadisticas<DtoHistoriaParaEstadisticas>
    {
        private readonly IRepositorioEstadisticasFinales _repositorio;

        public ListarHistoriasParaEstadisticas(IRepositorioEstadisticasFinales repositorio)
        {
            _repositorio = repositorio;
        }

        public IEnumerable<DtoHistoriaParaEstadisticas> Ejecutar()
        {
            return _repositorio.ObtenerHistorias()
                .Select(h => new DtoHistoriaParaEstadisticas(h.Id, h.Titulo.Value))
                .ToList();
        }
    }
}
