using ObligatorioP3.LogicaNegocio.Entidades;

namespace ObligatorioP3.LogicaNegocio.InterfacesRepositorio
{
    public interface IRepositorioLectura :
        IAdd<Lectura>,
        IDelete<Lectura>,
        IGetAll<Lectura>,
        IGetById<Lectura>,
        IUpdate<Lectura>
    {
        bool ExisteLectura(Lectura lectura);
        Lectura ObtenerParaUsuario(int lecturaId, int usuarioId);
        IEnumerable<Lectura> ObtenerHistorial(int usuarioId, int mes, int anio);
        void GuardarCambios();
    }
}
