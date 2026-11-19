using ObligatorioP3.LogicaNegocio.Excepciones;
using System;
using System.Collections.Generic;
using System.Text;

namespace ObligatorioP3.LogicaNegocio.Entidades
{
    public class Lectura
    {
        public int Id { get; private set; }
        public Usuario Usuario { get; private set; } = null!;
        public Historia Historia { get; private set; } = null!;
        public DateTime FechaInicio { get; private set; }
        public DateTime? FechaFinalizacion { get; private set; }
        public CapituloFinal? FinalAlcanzado { get; private set; }
        public Capitulo CapituloActual { get; private set; } = null!;
        public bool Completada => FechaFinalizacion.HasValue;

        protected Lectura() { }

        public Lectura(Usuario usuario, Historia historia)
        {
            Usuario = usuario;
            Historia = historia;
            if (historia is null || historia.Estado != EstadoHistoria.PUBLICADA)
                throw new LecturaInvalidaException("Solo se pueden leer historias publicadas.");
            CapituloActual = historia.CapituloInicial;
            FechaInicio = DateTime.UtcNow;
        }

        public void ElegirOpcion(Opcion opcion)
        {
            if (Completada)
                throw new LecturaInvalidaException("La lectura ya fue completada.");
            if (CapituloActual is not CapituloIntermedio intermedio || opcion is null || !intermedio.Opciones.Contains(opcion))
                throw new LecturaInvalidaException("La opción no corresponde al capítulo actual.");

            CapituloActual = opcion.Destino;
            if (CapituloActual is CapituloFinal capituloFinal)
            {
                FinalAlcanzado = capituloFinal;
                FechaFinalizacion = DateTime.UtcNow;
                capituloFinal.RegistrarLlegada();
            }
        }
    }

}
