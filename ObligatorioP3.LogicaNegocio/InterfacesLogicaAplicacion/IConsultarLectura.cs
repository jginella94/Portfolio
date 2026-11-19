namespace ObligatorioP3.LogicaNegocio.InterfacesLogicaAplicacion
{
    public interface IConsultarLectura<T>
    {
        T Ejecutar(int lecturaId, int usuarioId);
    }
}
