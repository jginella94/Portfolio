namespace ObligatorioP3.LogicaNegocio.Excepciones
{
    public class LecturaInvalidaException : LogicaNegocioException
    {
        public LecturaInvalidaException() { }
        public LecturaInvalidaException(string mensaje) : base(mensaje) { }
    }
}
