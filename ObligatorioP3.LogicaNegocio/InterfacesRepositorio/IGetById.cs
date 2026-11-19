namespace ObligatorioP3.LogicaNegocio.InterfacesRepositorio
{
    public interface IGetById<T>
    {
        T GetById(int id);
    }
}
