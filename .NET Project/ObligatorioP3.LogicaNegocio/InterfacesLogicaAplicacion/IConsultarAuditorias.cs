namespace ObligatorioP3.LogicaNegocio.InterfacesLogicaAplicacion
{
    public interface IConsultarAuditorias<T>
    {
        IEnumerable<T> Ejecutar(int? historiaId, int? administradorId);
    }
}
