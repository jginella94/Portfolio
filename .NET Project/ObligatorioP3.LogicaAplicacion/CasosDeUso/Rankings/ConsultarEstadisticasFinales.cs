using ObligatorioP3.LogicaAplicacion.DTOs.Rankings;
using ObligatorioP3.LogicaNegocio.InterfacesLogicaAplicacion;
using ObligatorioP3.LogicaNegocio.InterfacesRepositorio;

namespace ObligatorioP3.LogicaAplicacion.CasosDeUso.Rankings
{
    public class ConsultarEstadisticasFinales : IConsultarEstadisticasFinales<DtoEstadisticasFinales>
    {
        private readonly IRepositorioEstadisticasFinales _repositorio;

        public ConsultarEstadisticasFinales(IRepositorioEstadisticasFinales repositorio)
        {
            _repositorio = repositorio;
        }

        public DtoEstadisticasFinales? Ejecutar(int historiaId)
        {
            var historia = _repositorio.ObtenerHistoria(historiaId);
            if (historia is null)
                return null;

            var frecuencias = _repositorio.ObtenerFrecuenciaFinales(historiaId).ToList();
            int maximo = frecuencias.Count == 0 ? 0 : frecuencias.Max(f => f.Cantidad);

            var finales = frecuencias
                .Select(f => new DtoFrecuenciaFinal(
                    f.Final.Id,
                    f.Final.Titulo.Value,
                    f.Cantidad,
                    maximo > 0 && f.Cantidad == maximo))
                .ToList();

            return new DtoEstadisticasFinales(historia.Id, historia.Titulo.Value, finales);
        }
    }
}
