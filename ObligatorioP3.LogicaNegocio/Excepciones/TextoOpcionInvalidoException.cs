namespace ObligatorioP3.LogicaNegocio.Excepciones
{
    public class TextoOpcionInvalidoException : LogicaNegocioException
    {
        public TextoOpcionInvalidoException() { }
        public TextoOpcionInvalidoException(string mensaje) : base(mensaje) { }
    }
}
