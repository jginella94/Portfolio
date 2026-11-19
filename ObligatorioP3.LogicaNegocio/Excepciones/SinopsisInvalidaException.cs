namespace ObligatorioP3.LogicaNegocio.Excepciones
{
    public class SinopsisInvalidaException : LogicaNegocioException
    {
        public SinopsisInvalidaException() { }
        public SinopsisInvalidaException(string mensaje) : base(mensaje) { }
    }
}
