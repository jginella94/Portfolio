using ObligatorioP3.LogicaAplicacion.DTOs.Historias;
using ObligatorioP3.LogicaNegocio.Entidades;
using ObligatorioP3.LogicaNegocio.Excepciones;
using ObligatorioP3.LogicaNegocio.InterfacesLogicaAplicacion;
using ObligatorioP3.LogicaNegocio.InterfacesRepositorio;
using ObligatorioP3.LogicaNegocio.Vo;
namespace ObligatorioP3.LogicaAplicacion.CasosDeUso.Historias
{
    public class EditarHistoria : IEditar<DtoHistoriaEdicion>
    {
        private readonly IRepositorioHistoria _historias;
        private readonly IRepositorioCategoria _categorias;

        public EditarHistoria(IRepositorioHistoria historias, IRepositorioCategoria categorias)
        {
            _historias = historias;
            _categorias = categorias;
        }

        public void Ejecutar(int id, DtoHistoriaEdicion dto)
        {
            if (dto is null || dto.Id != id)
                throw new LogicaNegocioException("Los datos de la historia no coinciden.");
            if (dto.CategoriasIds is null || dto.CategoriasIds.Count == 0 || dto.CategoriasIds.Any(x => x <= 0))
                throw new LogicaNegocioException("Debe seleccionar al menos una categoría válida.");
            if (!Enum.TryParse<EstadoHistoria>(dto.Estado, true, out var estado) || !Enum.IsDefined(estado))
                throw new LogicaNegocioException("El estado de la historia no es válido.");

            Historia historia = _historias.GetById(id);
            List<Categoria> categorias = dto.CategoriasIds.Distinct().Select(_categorias.GetById).ToList();
            if (historia.Estado == EstadoHistoria.PUBLICADA && estado == EstadoHistoria.BORRADOR)
                throw new LogicaNegocioException("No se puede volver a borrador con las operaciones actuales del dominio.");

            historia.Modificar(new VoTitulo(dto.Titulo), new VoSinopsis(dto.Sinopsis), categorias);
            if (estado == EstadoHistoria.PUBLICADA && historia.Estado != EstadoHistoria.PUBLICADA)
                historia.Publicar();
            _historias.Update(id, historia);
        }
    }
}
