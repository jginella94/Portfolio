namespace ObligatorioP3.LogicaAplicacion.DTOs.AuditoriasHistorias
{
    public record DtoAuditoriaHistoria(
        int Id,
        int HistoriaId,
        string Accion,
        DateTime Fecha,
        int UsuarioAdminId,
        string NombreUsuarioAdmin
    );
}