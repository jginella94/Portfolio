using ObligatorioP3.LogicaAplicacion.DTOs.Lecturas;
using ObligatorioP3.LogicaNegocio.Entidades;

namespace ObligatorioP3.LogicaAplicacion.Mappers
{
    public static class LecturaHistorialMapper
    {
        public static DtoLecturaHistorial DesdeEntidad(Lectura lectura)
        {
            return new DtoLecturaHistorial
            {
                LecturaId = lectura.Id,
                HistoriaId = lectura.Historia.Id,
                TituloHistoria = lectura.Historia.Titulo.Value,
                FechaInicio = lectura.FechaInicio,
                FechaFinalizacion = lectura.FechaFinalizacion,
                Completada = lectura.Completada,
                FinalAlcanzadoId = lectura.FinalAlcanzado?.Id,
                TituloFinal = lectura.FinalAlcanzado?.Titulo.Value
            };
        }

        public static IEnumerable<DtoLecturaHistorial> ListadoDto(IEnumerable<Lectura> lecturas)
        {
            return lecturas.Select(DesdeEntidad).ToList();
        }
    }
}
