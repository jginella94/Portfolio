using Obligatorio.LogicaAplicacion.Mapper;
using ObligatorioP3.LogicaAplicacion.DTOs.AuditoriasHistorias;
using ObligatorioP3.LogicaNegocio.InterfacesLogicaAplicacion;
using ObligatorioP3.LogicaNegocio.InterfacesRepositorio;

namespace ObligatorioP3.LogicaAplicacion.CasosDeUso.AuditoriasHistorias
{
    public class ConsultarAuditoriasHistoria : IConsultarAuditorias<DtoAuditoriaHistoria>
    {
        private readonly IRepositorioAuditoriaHistoria _repositorio;

        public ConsultarAuditoriasHistoria(IRepositorioAuditoriaHistoria repositorio)
        {
            _repositorio = repositorio;
        }

        public IEnumerable<DtoAuditoriaHistoria> Ejecutar(int? historiaId, int? administradorId)
        {
            if (historiaId.HasValue && historiaId.Value <= 0)
                throw new ArgumentException("El identificador de historia debe ser positivo.");

            if (administradorId.HasValue && administradorId.Value <= 0)
                throw new ArgumentException("El identificador del administrador debe ser positivo.");

            return AuditoriaHistoriaMapper.ListadoDto(_repositorio.ObtenerPorFiltros(historiaId, administradorId));
        }
    }
}
