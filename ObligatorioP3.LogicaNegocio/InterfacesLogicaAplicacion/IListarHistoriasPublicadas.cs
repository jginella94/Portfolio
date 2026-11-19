namespace ObligatorioP3.LogicaNegocio.InterfacesLogicaAplicacion
{
    public interface IListarHistoriasPublicadas<T>
    {
        IEnumerable<T> Ejecutar(int? categoriaId);
    }
}
