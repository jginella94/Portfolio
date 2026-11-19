namespace ObligatorioP3.LogicaAplicacion.DTOs.Lecturas
{
    public record DtoLecturaListado(
        int Id,
        int HistoriaId,
        string TituloHistoria,
        DateTime FechaInicio,
        DateTime? FechaFinalizacion,
        bool Completada,
        string? TituloFinalAlcanzado
    );
}