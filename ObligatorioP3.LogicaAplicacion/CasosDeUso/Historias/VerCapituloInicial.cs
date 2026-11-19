using ObligatorioP3.LogicaAplicacion.DTOs.Historias;
using ObligatorioP3.LogicaNegocio.InterfacesLogicaAplicacion;
using ObligatorioP3.LogicaNegocio.InterfacesRepositorio;

namespace ObligatorioP3.LogicaAplicacion.CasosDeUso.Historias
{
    public class VerCapituloInicial : IVer<DtoCapituloInicialEdicion>
    {
        private readonly IRepositorioHistoria _historias;

        public VerCapituloInicial(IRepositorioHistoria historias)
        {
            _historias = historias;
        }

        public DtoCapituloInicialEdicion Ejecutar(int id)
        {
            var historia = _historias.GetById(id);
            return new DtoCapituloInicialEdicion
            {
                HistoriaId = historia.Id,
                Titulo = historia.CapituloInicial!.Titulo.Value,
                Texto = historia.CapituloInicial.Texto.Value
            };
        }
    }
}
