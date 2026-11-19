using ObligatorioP3.LogicaAplicacion.DTOs.Lecturas;
using ObligatorioP3.LogicaAplicacion.Mappers;
using ObligatorioP3.LogicaNegocio.Entidades;
using ObligatorioP3.LogicaNegocio.Excepciones;
using ObligatorioP3.LogicaNegocio.InterfacesLogicaAplicacion;
using ObligatorioP3.LogicaNegocio.InterfacesRepositorio;

namespace ObligatorioP3.LogicaAplicacion.CasosDeUso.Lecturas
{
    public class ElegirOpcionLectura : IElegirOpcionLectura<DtoLecturaActual>
    {
        private readonly IRepositorioLectura _lecturas;

        public ElegirOpcionLectura(IRepositorioLectura lecturas)
        {
            _lecturas = lecturas;
        }

        public DtoLecturaActual Ejecutar(int lecturaId, int opcionId, int usuarioId)
        {
            Lectura lectura = _lecturas.ObtenerParaUsuario(lecturaId, usuarioId);

            if (lectura.Completada)
                throw new LecturaInvalidaException("La lectura ya fue completada.");

            if (lectura.CapituloActual is not CapituloIntermedio intermedio)
                throw new LecturaInvalidaException("El capítulo actual no permite elegir opciones.");

            Opcion? opcion = intermedio.Opciones.FirstOrDefault(o => o.Id == opcionId);
            if (opcion is null)
                throw new LecturaInvalidaException("La opción no corresponde al capítulo actual.");

            lectura.ElegirOpcion(opcion);
            _lecturas.GuardarCambios();

            if (!lectura.Completada)
                return LecturaMapper.DesdeEntidad(_lecturas.ObtenerParaUsuario(lecturaId, usuarioId));

            return LecturaMapper.DesdeEntidad(lectura);
        }
    }
}
