using ObligatorioP3.LogicaAplicacion.DTOs.Lecturas;
using ObligatorioP3.LogicaAplicacion.Mappers;
using ObligatorioP3.LogicaNegocio.InterfacesLogicaAplicacion;
using ObligatorioP3.LogicaNegocio.InterfacesRepositorio;

namespace ObligatorioP3.LogicaAplicacion.CasosDeUso.Lecturas
{
    public class ConsultarHistorialLecturas : IConsultarHistorialLecturas<DtoLecturaHistorial>
    {
        private readonly IRepositorioLectura _lecturas;

        public ConsultarHistorialLecturas(IRepositorioLectura lecturas)
        {
            _lecturas = lecturas;
        }

        public IEnumerable<DtoLecturaHistorial> Ejecutar(int usuarioId, int mes, int anio)
        {
            if (usuarioId <= 0 || mes < 1 || mes > 12 || anio < 1 || anio > 9998)
                throw new ArgumentException("El usuario, mes o año no son válidos.");

            return LecturaHistorialMapper.ListadoDto(_lecturas.ObtenerHistorial(usuarioId, mes, anio));
        }
    }
}
