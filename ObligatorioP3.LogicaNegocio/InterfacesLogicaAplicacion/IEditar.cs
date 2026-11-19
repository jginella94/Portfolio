
namespace ObligatorioP3.LogicaNegocio.InterfacesLogicaAplicacion
{
    public interface IEditar<T>
    {
        void Ejecutar(int id, T obj);
    }
}
