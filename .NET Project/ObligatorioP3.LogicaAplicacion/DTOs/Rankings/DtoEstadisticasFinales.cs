namespace ObligatorioP3.LogicaAplicacion.DTOs.Rankings
{
    public record DtoEstadisticasFinales(int HistoriaId, string Titulo, IEnumerable<DtoFrecuenciaFinal> Finales);
}
