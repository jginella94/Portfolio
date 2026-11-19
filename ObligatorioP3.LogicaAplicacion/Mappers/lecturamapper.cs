using ObligatorioP3.LogicaAplicacion.DTOs.Lecturas;
using ObligatorioP3.LogicaNegocio.Entidades;

namespace ObligatorioP3.LogicaAplicacion.Mappers
{
    public static class LecturaMapper
    {
        public static DtoLecturaActual DesdeEntidad(Lectura lectura)
        {
            DtoLecturaActual dto = new()
            {
                LecturaId = lectura.Id,
                HistoriaId = lectura.Historia.Id,
                CapituloId = lectura.CapituloActual.Id,
                Titulo = lectura.CapituloActual.Titulo.Value,
                Texto = lectura.CapituloActual.Texto.Value,
                Completada = lectura.Completada,
                FechaInicio = lectura.FechaInicio,
                FechaFinalizacion = lectura.FechaFinalizacion,
                FinalAlcanzadoId = lectura.FinalAlcanzado?.Id
            };

            if (lectura.CapituloActual is CapituloIntermedio intermedio)
            {
                foreach (Opcion opcion in intermedio.Opciones)
                {
                    dto.Opciones.Add(new DtoOpcionLectura
                    {
                        Id = opcion.Id,
                        Texto = opcion.Texto.Value
                    });
                }
            }

            return dto;
        }
    }
}
