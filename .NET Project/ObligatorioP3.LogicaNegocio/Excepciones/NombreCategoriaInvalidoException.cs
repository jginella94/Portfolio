namespace ObligatorioP3.LogicaNegocio.Excepciones
{
    public class NombreCategoriaInvalidoException : LogicaNegocioException
    {
        public NombreCategoriaInvalidoException() { }
        public NombreCategoriaInvalidoException(string mensaje) : base(mensaje) { }
    }
}
