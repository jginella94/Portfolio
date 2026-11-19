namespace ObligatorioP3.LogicaNegocio.Excepciones
{
    public class AuditoriaInvalidaException : LogicaNegocioException
    {
        public AuditoriaInvalidaException() { }
        public AuditoriaInvalidaException(string mensaje) : base(mensaje) { }
    }
}
