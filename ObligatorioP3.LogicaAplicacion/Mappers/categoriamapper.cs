using ObligatorioP3.LogicaAplicacion.DTOs.Categorias;
using ObligatorioP3.LogicaNegocio.Entidades;
using ObligatorioP3.LogicaNegocio.Vo;

namespace ObligatorioP3.LogicaAplicacion.Mapper
{
    public static class CategoriaMapper
    {
        public static Categoria DesdeDto(DtoCategoria dto)
        {
            return new Categoria(new VoNombreCategoria(dto.Nombre));
        }

        public static DtoCategoria DesdeEntidad(Categoria categoria)
        {
            return new DtoCategoria
            {
                Id = categoria.Id,
                Nombre = categoria.Nombre.Value
            };
        }

        public static IEnumerable<DtoCategoria> ListadoDto(IEnumerable<Categoria> categorias)
        {
            return categorias.Select(DesdeEntidad).ToList();
        }
    }
}