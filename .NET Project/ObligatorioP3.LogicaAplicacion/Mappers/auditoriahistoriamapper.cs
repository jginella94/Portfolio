using ObligatorioP3.LogicaAplicacion.DTOs.AuditoriasHistorias;
using ObligatorioP3.LogicaNegocio.Entidades;

namespace Obligatorio.LogicaAplicacion.Mapper
{
    public static class AuditoriaHistoriaMapper
    {
        public static DtoAuditoriaHistoria DesdeEntidad(AuditoriaHistoria auditoria)
        {
            return new DtoAuditoriaHistoria(
                Id: auditoria.Id,
                HistoriaId: auditoria.HistoriaIdHistorico,
                Accion: auditoria.Accion,
                Fecha: auditoria.Fecha,
                UsuarioAdminId: auditoria.Administrador.Id,
                NombreUsuarioAdmin: auditoria.Administrador.NombreCompleto.Value
            );
        }

        public static IEnumerable<DtoAuditoriaHistoria> ListadoDto(IEnumerable<AuditoriaHistoria> auditorias)
        {
            return auditorias.Select(DesdeEntidad).ToList();
        }
    }
}