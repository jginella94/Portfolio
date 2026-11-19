namespace ObligatorioP3.LogicaNegocio.InterfacesLogicaAplicacion
{
    public interface IRegistrarAuditoriaHistoria
    {
        int Ejecutar(int historiaId, int administradorId, string accion);
    }
}
