using ObligatorioP3.LogicaNegocio.Excepciones;

namespace ObligatorioP3.LogicaNegocio.Vo
{
    public record VoSinopsis
    {
        public string Value { get; private set; }

        private VoSinopsis() { Value = string.Empty; }

        public VoSinopsis(string value)
        {
            Value = value?.Trim() ?? string.Empty;
            Validar();
        }

        private void Validar()
        {
            if (string.IsNullOrWhiteSpace(Value))
                throw new SinopsisInvalidaException("La sinopsis no puede estar vacío.");
        }
    }
}
