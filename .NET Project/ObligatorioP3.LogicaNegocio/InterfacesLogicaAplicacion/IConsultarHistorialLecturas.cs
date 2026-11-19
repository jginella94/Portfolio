namespace ObligatorioP3.LogicaNegocio.InterfacesLogicaAplicacion
{
    public interface IConsultarHistorialLecturas<T>
    {
        IEnumerable<T> Ejecutar(int usuarioId, int mes, int anio);
    }
}
