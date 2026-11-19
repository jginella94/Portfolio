namespace ObligatorioP3.LogicaNegocio.InterfacesLogicaAplicacion
{
    public interface IObtenerRankingLectores<T>
    {
        IEnumerable<T> Ejecutar();
    }
}
