using ObligatorioP3.LogicaNegocio.Excepciones;

namespace ObligatorioP3.LogicaNegocio.Vo
{
    public record VoTextoOpcion
    {
        public string Value { get; private set; }

        private VoTextoOpcion() { Value = string.Empty; }

        public VoTextoOpcion(string value)
        {
            Value = value?.Trim() ?? string.Empty;
            Validar();
        }

        private void Validar()
        {
            if (string.IsNullOrWhiteSpace(Value))
                throw new TextoOpcionInvalidoException("El texto de la opción no puede estar vacío.");
        }
    }
}
