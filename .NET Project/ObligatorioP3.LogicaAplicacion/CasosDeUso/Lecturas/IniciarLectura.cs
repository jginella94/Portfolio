using ObligatorioP3.LogicaAplicacion.DTOs.Lecturas;
using ObligatorioP3.LogicaAplicacion.Mappers;
using ObligatorioP3.LogicaNegocio.Entidades;
using ObligatorioP3.LogicaNegocio.Excepciones;
using ObligatorioP3.LogicaNegocio.InterfacesLogicaAplicacion;
using ObligatorioP3.LogicaNegocio.InterfacesRepositorio;

namespace ObligatorioP3.LogicaAplicacion.CasosDeUso.Lecturas
{
    public class IniciarLectura : IIniciarLectura<DtoLecturaActual>
    {
        private readonly IRepositorioUsuario _usuarios;
        private readonly IRepositorioHistoria _historias;
        private readonly IRepositorioLectura _lecturas;

        public IniciarLectura(IRepositorioUsuario usuarios, IRepositorioHistoria historias, IRepositorioLectura lecturas)
        {
            _usuarios = usuarios;
            _historias = historias;
            _lecturas = lecturas;
        }

        public DtoLecturaActual Ejecutar(int historiaId, int usuarioId)
        {
            Usuario usuario = _usuarios.GetById(usuarioId);
            if (usuario is not Lector)
                throw new LecturaInvalidaException("Solo los lectores pueden iniciar lecturas.");

            Historia historia = _historias.GetById(historiaId);
            Lectura lectura = new(usuario, historia);
            _lecturas.Add(lectura);
            return LecturaMapper.DesdeEntidad(lectura);
        }
    }
}
