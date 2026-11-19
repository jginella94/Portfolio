namespace ObligatorioP3.LogicaNegocio.InterfacesLogicaAplicacion
{
    public interface IObtenerRankingHistorias<T>
    {
        IEnumerable<T> Ejecutar();
    }
}
