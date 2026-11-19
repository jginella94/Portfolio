namespace ObligatorioP3.LogicaNegocio.InterfacesLogicaAplicacion
{
    public interface IAutenticar
    {
        int? Ejecutar(string nombreUsuario, string password);
    }
}
