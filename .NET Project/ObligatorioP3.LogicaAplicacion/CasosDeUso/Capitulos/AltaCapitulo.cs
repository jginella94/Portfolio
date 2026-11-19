using ObligatorioP3.LogicaAplicacion.DTOs.Capitulos;
using ObligatorioP3.LogicaNegocio.Entidades;
using ObligatorioP3.LogicaNegocio.Excepciones;
using ObligatorioP3.LogicaNegocio.InterfacesLogicaAplicacion;
using ObligatorioP3.LogicaNegocio.InterfacesRepositorio;
using ObligatorioP3.LogicaNegocio.Vo;

namespace ObligatorioP3.LogicaAplicacion.CasosDeUso.Capitulos
{
    public class AltaCapitulo : IAlta<DtoCapituloAgregar>
    {
        private readonly IRepositorioHistoria _historias;

        public AltaCapitulo(IRepositorioHistoria historias)
        {
            _historias = historias;
        }

        public int Ejecutar(DtoCapituloAgregar dto)
        {
            if (dto is null || dto.HistoriaId <= 0 || dto.CapituloAntecedenteId <= 0)
                throw new LogicaNegocioException("La historia y el capítulo antecedente son obligatorios.");

            Historia historia = _historias.GetById(dto.HistoriaId);

            CapituloIntermedio? antecedente = historia.Capitulos
                .OfType<CapituloIntermedio>()
                .FirstOrDefault(c => c.Id == dto.CapituloAntecedenteId);

            if (antecedente is null)
                throw new LogicaNegocioException("El capítulo antecedente no existe o no es intermedio de la historia.");

            VoTitulo titulo = new VoTitulo(dto.Titulo);
            VoTexto texto = new VoTexto(dto.Texto);
            VoTextoOpcion textoOpcion = new VoTextoOpcion(dto.TextoOpcion);

            Capitulo nuevoCapitulo = dto.Tipo?.Trim().ToLowerInvariant() switch
            {
                "intermedio" => new CapituloIntermedio(titulo, texto),
                "final" => new CapituloFinal(titulo, texto),
                _ => throw new LogicaNegocioException("El tipo de capítulo debe ser Intermedio o Final.")
            };

            historia.AgregarCapitulo(antecedente, nuevoCapitulo, textoOpcion);
            _historias.Update(historia.Id, historia);

            return nuevoCapitulo.Id;
        }
    }
}
