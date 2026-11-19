using ObligatorioP3.LogicaNegocio.Excepciones;

namespace ObligatorioP3.LogicaNegocio.Vo
{
    public record VoTitulo
    {
        public string Value { get; private set; }

        public VoTitulo(string value)
        {
            Value = value;
            Validar();
        }

        private void Validar()
        {
            if (string.IsNullOrWhiteSpace(Value))
                throw new TituloInvalidoException("El título no puede estar vacío.");
        }
    }
}
