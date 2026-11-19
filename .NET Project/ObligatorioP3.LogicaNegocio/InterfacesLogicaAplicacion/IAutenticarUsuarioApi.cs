namespace ObligatorioP3.LogicaNegocio.InterfacesLogicaAplicacion
{
    public interface IAutenticarUsuarioApi<T>
    {
        T? Ejecutar(string nombreUsuario, string password);
    }
}
