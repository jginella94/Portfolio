using ObligatorioP3.LogicaAplicacion.DTOs.Historias;
using ObligatorioP3.LogicaNegocio.Entidades;
using ObligatorioP3.LogicaNegocio.Excepciones;
using ObligatorioP3.LogicaNegocio.InterfacesLogicaAplicacion;
using ObligatorioP3.LogicaNegocio.InterfacesRepositorio;
using ObligatorioP3.LogicaNegocio.Vo;
namespace ObligatorioP3.LogicaAplicacion.CasosDeUso.Historias
{
    public class EditarCapituloInicial : IEditar<DtoCapituloInicialEdicion>
    {
        private readonly IRepositorioHistoria _historias;

        public EditarCapituloInicial(IRepositorioHistoria historias)
        {
            _historias = historias;
        }

        public void Ejecutar(int id, DtoCapituloInicialEdicion dto)
        {
            if (dto is null || dto.HistoriaId != id)
                throw new LogicaNegocioException("Los datos del capítulo inicial no coinciden.");

            Historia historia = _historias.GetById(id);
            historia.ModificarCapituloInicial(new VoTitulo(dto.Titulo), new VoTexto(dto.Texto));
            _historias.Update(id, historia);
        }
    }
}
