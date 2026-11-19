using ObligatorioP3.LogicaNegocio.Entidades;

namespace ObligatorioP3.LogicaNegocio.InterfacesRepositorio
{
    public interface IRepositorioEstadisticasFinales
    {
        Historia? ObtenerHistoria(int historiaId);
        IEnumerable<Historia> ObtenerHistorias();
        IEnumerable<(CapituloFinal Final, int Cantidad)> ObtenerFrecuenciaFinales(int historiaId);
    }
}
