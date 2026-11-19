using ObligatorioP3.LogicaAplicacion.DTOs.Historias;
using ObligatorioP3.LogicaAplicacion.Mapper;
using ObligatorioP3.LogicaNegocio.InterfacesLogicaAplicacion;
using ObligatorioP3.LogicaNegocio.InterfacesRepositorio;

namespace ObligatorioP3.LogicaAplicacion.CasosDeUso.Historias
{
    public class VerHistoria : IVer<DtoHistoriaListado>
    {
        private readonly IRepositorioHistoria _historias;

        public VerHistoria(IRepositorioHistoria historias)
        {
            _historias = historias;
        }

        public DtoHistoriaListado Ejecutar(int id)
        {
            return HistoriaMapper.DesdeEntidad(_historias.GetById(id));
        }
    }
}
