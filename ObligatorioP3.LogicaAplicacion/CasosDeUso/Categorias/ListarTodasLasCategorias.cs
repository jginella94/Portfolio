using ObligatorioP3.LogicaAplicacion.DTOs.Categorias;
using ObligatorioP3.LogicaAplicacion.Mapper;
using ObligatorioP3.LogicaNegocio.InterfacesLogicaAplicacion;
using ObligatorioP3.LogicaNegocio.InterfacesRepositorio;

namespace ObligatorioP3.LogicaAplicacion.CasosDeUso.Categorias
{
    public class ListarTodasLasCategorias : IListarTodos<DtoCategoria>
    {
        private readonly IRepositorioCategoria _categorias;

        public ListarTodasLasCategorias(IRepositorioCategoria categorias)
        {
            _categorias = categorias;
        }

        public IEnumerable<DtoCategoria> Ejecutar()
        {
            return CategoriaMapper.ListadoDto(_categorias.GetAll());
        }
    }
}
