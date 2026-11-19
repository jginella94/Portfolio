namespace ObligatorioP3.LogicaNegocio.Excepciones
{
    public class TituloInvalidoException : LogicaNegocioException
    {
        public TituloInvalidoException() { }
        public TituloInvalidoException(string mensaje) : base(mensaje) { }
    }
}
