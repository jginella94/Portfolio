namespace ObligatorioP3.LogicaNegocio.Excepciones
{
    public class TextoInvalidoException : LogicaNegocioException
    {
        public TextoInvalidoException() { }
        public TextoInvalidoException(string mensaje) : base(mensaje) { }
    }
}
