namespace ObligatorioP3.LogicaNegocio.InterfacesRepositorio
{
    public interface IRepositorioBajaHistoriaAuditada
    {
        void DeleteConAuditoria(int historiaId, int administradorId);
    }
}
