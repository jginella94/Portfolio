using ObligatorioP3.LogicaAplicacion.DTOs.Historias;
using ObligatorioP3.LogicaAplicacion.Mapper;
using ObligatorioP3.LogicaNegocio.InterfacesLogicaAplicacion;
using ObligatorioP3.LogicaNegocio.InterfacesRepositorio;

namespace ObligatorioP3.LogicaAplicacion.CasosDeUso.Historias
{
    public class ListarTodasLasHistorias : IListarTodos<DtoHistoriaListado>
    {
        private readonly IRepositorioHistoria _historias;

        public ListarTodasLasHistorias(IRepositorioHistoria historias)
        {
            _historias = historias;
        }

        public IEnumerable<DtoHistoriaListado> Ejecutar()
        {
            return HistoriaMapper.ListadoDto(_historias.GetAll());
        }
    }
}
