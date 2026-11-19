using ObligatorioP3.LogicaAplicacion.DTOs.Capitulos;
using ObligatorioP3.LogicaNegocio.Entidades;
using ObligatorioP3.LogicaNegocio.InterfacesLogicaAplicacion;
using ObligatorioP3.LogicaNegocio.InterfacesRepositorio;

namespace ObligatorioP3.LogicaAplicacion.CasosDeUso.Capitulos
{
    public class VerCapitulosParaAlta : IVer<DtoCapitulosParaAlta>
    {
        private readonly IRepositorioHistoria _historias;

        public VerCapitulosParaAlta(IRepositorioHistoria historias)
        {
            _historias = historias;
        }

        public DtoCapitulosParaAlta Ejecutar(int id)
        {
            Historia historia = _historias.GetById(id);

            return new DtoCapitulosParaAlta
            {
                HistoriaId = historia.Id,
                TituloHistoria = historia.Titulo.Value,
                Estado = historia.Estado.ToString(),
                CapitulosIntermedios = historia.Capitulos
                    .OfType<CapituloIntermedio>()
                    .Select(c => new DtoCapituloAntecedente
                    {
                        Id = c.Id,
                        Titulo = c.Titulo.Value
                    })
                    .OrderBy(c => c.Id)
                    .ToList()
            };
        }
    }
}
