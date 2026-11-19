using ObligatorioP3.LogicaAplicacion.DTOs.Historias;
using ObligatorioP3.LogicaNegocio.Entidades;

namespace ObligatorioP3.LogicaAplicacion.Mapper
{
    public static class HistoriaMapper
    {
        public static DtoHistoriaListado DesdeEntidad(Historia historia)
        {
            return new DtoHistoriaListado(
                Id: historia.Id,
                Titulo: historia.Titulo.Value,
                Sinopsis: historia.Sinopsis.Value,
                Estado: historia.Estado.ToString(),
                Categorias: historia.Categorias.Select(c => c.Id).ToList(), 
                IdCapituloInicial: historia.CapituloInicial.Id
                
            );
        }

        public static IEnumerable<DtoHistoriaListado> ListadoDto(IEnumerable<Historia> historias)
        {
            List<DtoHistoriaListado> aux = new List<DtoHistoriaListado>();
            foreach (Historia historia in historias)
            {
                aux.Add(DesdeEntidad(historia));
            }
            return aux;
        }
    }
}