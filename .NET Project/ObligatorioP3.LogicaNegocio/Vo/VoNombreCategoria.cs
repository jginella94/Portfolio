using ObligatorioP3.LogicaNegocio.Excepciones;

namespace ObligatorioP3.LogicaNegocio.Vo
{
    public record VoNombreCategoria
    {
        public string Value { get; private set; }

        private VoNombreCategoria() { Value = string.Empty; }

        public VoNombreCategoria(string value)
        {
            Value = value?.Trim() ?? string.Empty;
            Validar();
        }

        private void Validar()
        {
            if (string.IsNullOrWhiteSpace(Value))
                throw new NombreCategoriaInvalidoException("El nombre de categoría no puede estar vacío.");
        }
    }
}
