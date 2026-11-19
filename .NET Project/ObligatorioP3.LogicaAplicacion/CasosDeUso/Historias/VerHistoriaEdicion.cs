using ObligatorioP3.LogicaAplicacion.DTOs.Historias;
using ObligatorioP3.LogicaNegocio.InterfacesLogicaAplicacion;
using ObligatorioP3.LogicaNegocio.InterfacesRepositorio;

namespace ObligatorioP3.LogicaAplicacion.CasosDeUso.Historias
{
    public class VerHistoriaEdicion : IVer<DtoHistoriaEdicion>
    {
        private readonly IRepositorioHistoria _historias;

        public VerHistoriaEdicion(IRepositorioHistoria historias)
        {
            _historias = historias;
        }

        public DtoHistoriaEdicion Ejecutar(int id)
        {
            var historia = _historias.GetById(id);
            return new DtoHistoriaEdicion
            {
                Id = historia.Id,
                Titulo = historia.Titulo.Value,
                Sinopsis = historia.Sinopsis.Value,
                Estado = historia.Estado.ToString(),
                CategoriasIds = historia.Categorias.Select(c => c.Id).ToList()
            };
        }
    }
}
