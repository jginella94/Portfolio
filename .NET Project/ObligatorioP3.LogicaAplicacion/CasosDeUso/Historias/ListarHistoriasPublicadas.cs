using ObligatorioP3.LogicaAplicacion.DTOs.Historias;
using ObligatorioP3.LogicaNegocio.Entidades;
using ObligatorioP3.LogicaNegocio.InterfacesLogicaAplicacion;
using ObligatorioP3.LogicaNegocio.InterfacesRepositorio;

namespace ObligatorioP3.LogicaAplicacion.CasosDeUso.Historias
{
    public class ListarHistoriasPublicadas : IListarHistoriasPublicadas<DtoHistoriaPublicada>
    {
        private readonly IRepositorioHistoriasPublicadas _historias;

        public ListarHistoriasPublicadas(IRepositorioHistoriasPublicadas historias)
        {
            _historias = historias;
        }

        public IEnumerable<DtoHistoriaPublicada> Ejecutar(int? categoriaId)
        {
            List<DtoHistoriaPublicada> resultado = new();

            foreach (Historia historia in _historias.ObtenerPublicadas(categoriaId))
            {
                DtoHistoriaPublicada dto = new()
                {
                    Id = historia.Id,
                    Titulo = historia.Titulo.Value,
                    Sinopsis = historia.Sinopsis.Value
                };

                foreach (Categoria categoria in historia.Categorias)
                    dto.Categorias.Add(categoria.Nombre.Value);

                resultado.Add(dto);
            }

            return resultado;
        }
    }
}
