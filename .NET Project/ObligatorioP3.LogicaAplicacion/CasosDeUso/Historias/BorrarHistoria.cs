using ObligatorioP3.LogicaAplicacion.DTOs.Historias;
using ObligatorioP3.LogicaNegocio.InterfacesLogicaAplicacion;
using ObligatorioP3.LogicaNegocio.InterfacesRepositorio;

namespace ObligatorioP3.LogicaAplicacion.CasosDeUso.Historias
{
    public class BorrarHistoria : IBorrar<DtoHistoriaListado>, IBorrarHistoriaAuditada
    {
        private readonly IRepositorioHistoria _historias;
        private readonly IRepositorioBajaHistoriaAuditada _bajaAuditada;

        public BorrarHistoria(IRepositorioHistoria historias, IRepositorioBajaHistoriaAuditada bajaAuditada)
        {
            _historias = historias;
            _bajaAuditada = bajaAuditada;
        }

        public void Ejecutar(int id)
        {
            _historias.Delete(id);
        }

        public void Ejecutar(int historiaId, int administradorId)
        {
            _bajaAuditada.DeleteConAuditoria(historiaId, administradorId);
        }
    }
}
