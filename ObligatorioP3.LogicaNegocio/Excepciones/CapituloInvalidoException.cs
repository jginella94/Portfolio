namespace ObligatorioP3.LogicaNegocio.Excepciones
{
    public class CapituloInvalidoException : LogicaNegocioException
    {
        public CapituloInvalidoException() { }
        public CapituloInvalidoException(string mensaje) : base(mensaje) { }
    }
}
