namespace ObligatorioP3.Infraestructura.AccesoDatos.EF.Excepciones
{
    public class NotFoundException : AccesoDatosException
    {
        public NotFoundException(string mensaje) : base(mensaje) { }
    }
}
