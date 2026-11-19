namespace ObligatorioP3.Infraestructura.AccesoDatos.EF.Excepciones
{
    public class AccesoDatosException : Exception
    {
        public AccesoDatosException(string mensaje) : base(mensaje) { }
    }
}
