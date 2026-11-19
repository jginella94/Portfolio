using ObligatorioP3.LogicaAplicacion.DTOs.Capitulos;
using ObligatorioP3.LogicaNegocio.Entidades;


namespace ObligatorioP3.LogicaAplicacion.Mapper
{
    public static class CapituloMapper
    {

        public static DtoCapituloDetalle DesdeEntidad(Capitulo capitulo)
        {
            var dto = new DtoCapituloDetalle
            {
                Id = capitulo.Id,
                Titulo = capitulo.Titulo.Value,
                Texto = capitulo.Texto.Value
            };

            if (capitulo is CapituloIntermedio intermedio)
            {
                dto.TipoCapitulo = "Intermedio";
                dto.Opciones = intermedio.Opciones?.Select(o => new DtoOpcion
                {
                    Id = o.Id,
                    Texto = o.Texto.Value,
                    CapituloDestinoId = o.Destino.Id
                }).ToList() ?? new List<DtoOpcion>();
            }
            else if (capitulo is CapituloFinal finalCap)
            {
                dto.TipoCapitulo = "Final";
                dto.ContadorVecesAlcanzado = finalCap.VecesAlcanzado;
            }

            return dto;
        }
    }
}