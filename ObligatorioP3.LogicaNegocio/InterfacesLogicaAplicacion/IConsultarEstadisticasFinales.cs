namespace ObligatorioP3.LogicaNegocio.InterfacesLogicaAplicacion
{
    public interface IConsultarEstadisticasFinales<T>
    {
        T? Ejecutar(int historiaId);
    }
}
