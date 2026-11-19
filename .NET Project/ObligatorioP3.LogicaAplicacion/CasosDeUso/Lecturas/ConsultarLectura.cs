using ObligatorioP3.LogicaAplicacion.DTOs.Lecturas;
using ObligatorioP3.LogicaAplicacion.Mappers;
using ObligatorioP3.LogicaNegocio.InterfacesLogicaAplicacion;
using ObligatorioP3.LogicaNegocio.InterfacesRepositorio;

namespace ObligatorioP3.LogicaAplicacion.CasosDeUso.Lecturas
{
    public class ConsultarLectura : IConsultarLectura<DtoLecturaActual>
    {
        private readonly IRepositorioLectura _lecturas;

        public ConsultarLectura(IRepositorioLectura lecturas)
        {
            _lecturas = lecturas;
        }

        public DtoLecturaActual Ejecutar(int lecturaId, int usuarioId)
        {
            return LecturaMapper.DesdeEntidad(_lecturas.ObtenerParaUsuario(lecturaId, usuarioId));
        }
    }
}
