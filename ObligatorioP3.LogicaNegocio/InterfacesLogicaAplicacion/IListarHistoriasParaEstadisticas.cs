namespace ObligatorioP3.LogicaNegocio.InterfacesLogicaAplicacion
{
    public interface IListarHistoriasParaEstadisticas<T>
    {
        IEnumerable<T> Ejecutar();
    }
}
