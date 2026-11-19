namespace ObligatorioP3.LogicaNegocio.Excepciones
{
    public class OpcionInvalidaException : LogicaNegocioException
    {
        public OpcionInvalidaException() { }
        public OpcionInvalidaException(string mensaje) : base(mensaje) { }
    }
}
