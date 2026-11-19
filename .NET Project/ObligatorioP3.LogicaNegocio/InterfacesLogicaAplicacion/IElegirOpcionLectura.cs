namespace ObligatorioP3.LogicaNegocio.InterfacesLogicaAplicacion
{
    public interface IElegirOpcionLectura<T>
    {
        T Ejecutar(int lecturaId, int opcionId, int usuarioId);
    }
}
