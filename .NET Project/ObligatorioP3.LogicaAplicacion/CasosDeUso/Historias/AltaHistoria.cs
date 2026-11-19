using ObligatorioP3.LogicaAplicacion.DTOs.Historias;
using ObligatorioP3.LogicaNegocio.Entidades;
using ObligatorioP3.LogicaNegocio.Excepciones;
using ObligatorioP3.LogicaNegocio.InterfacesLogicaAplicacion;
using ObligatorioP3.LogicaNegocio.InterfacesRepositorio;
using ObligatorioP3.LogicaNegocio.Vo;
namespace ObligatorioP3.LogicaAplicacion.CasosDeUso.Historias
{
    public class AltaHistoria : IAlta<DtoHistoriaAlta>
    {
        private readonly IRepositorioHistoria _historias;
        private readonly IRepositorioCategoria _categorias;

        public AltaHistoria(IRepositorioHistoria historias, IRepositorioCategoria categorias)
        {
            _historias = historias;
            _categorias = categorias;
        }

        public int Ejecutar(DtoHistoriaAlta dto)
        {
            if (dto is null)
                throw new LogicaNegocioException("Debe ingresar los datos de la historia.");
            if (dto.CategoriasIds is null || dto.CategoriasIds.Count == 0 || dto.CategoriasIds.Any(id => id <= 0))
                throw new LogicaNegocioException("Debe seleccionar al menos una categoría válida.");

            List<Categoria> categorias = dto.CategoriasIds.Distinct().Select(_categorias.GetById).ToList();
            CapituloIntermedio inicial = new CapituloIntermedio(
                new VoTitulo(dto.TituloCapituloInicial), new VoTexto(dto.TextoCapituloInicial));
            Historia historia = new Historia(new VoTitulo(dto.Titulo), new VoSinopsis(dto.Sinopsis), categorias, inicial);
            return _historias.Add(historia);
        }
    }
}
