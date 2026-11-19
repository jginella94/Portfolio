namespace ObligatorioP3.LogicaNegocio.InterfacesLogicaAplicacion
{
    public interface IIniciarLectura<T>
    {
        T Ejecutar(int historiaId, int usuarioId);
    }
}
