namespace ObligatorioP3.LogicaNegocio.Excepciones
{
    public class HistoriaInvalidaException : LogicaNegocioException
    {
        public HistoriaInvalidaException() { }
        public HistoriaInvalidaException(string mensaje) : base(mensaje) { }
    }
}
