using ObligatorioP3.LogicaNegocio.Excepciones;

namespace ObligatorioP3.LogicaNegocio.Vo
{
    public record VoTexto
    {
        public string Value { get; private set; }

        public VoTexto(string value)
        {
            Value = value;
            Validar();
        }

        private void Validar()
        {
            if (string.IsNullOrWhiteSpace(Value))
                throw new TextoInvalidoException("El texto no puede estar vacío.");
        }
    }
}
