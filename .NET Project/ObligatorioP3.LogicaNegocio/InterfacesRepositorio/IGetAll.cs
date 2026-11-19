namespace ObligatorioP3.LogicaNegocio.InterfacesRepositorio
{
    public interface IGetAll<T>
    {
        IEnumerable<T> GetAll();
    }
}
