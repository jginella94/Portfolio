using ObligatorioP3.LogicaNegocio.Entidades;
using ObligatorioP3.LogicaNegocio.Excepciones;
using ObligatorioP3.LogicaNegocio.InterfacesLogicaAplicacion;
using ObligatorioP3.LogicaNegocio.InterfacesRepositorio;

namespace ObligatorioP3.LogicaAplicacion.CasosDeUso.AuditoriasHistorias
{
    public class RegistrarAuditoriaHistoria : IRegistrarAuditoriaHistoria
    {
        private readonly IRepositorioAuditoriaHistoria _auditorias;
        private readonly IRepositorioHistoria _historias;
        private readonly IRepositorioUsuario _usuarios;

        public RegistrarAuditoriaHistoria(
            IRepositorioAuditoriaHistoria auditorias,
            IRepositorioHistoria historias,
            IRepositorioUsuario usuarios)
        {
            _auditorias = auditorias;
            _historias = historias;
            _usuarios = usuarios;
        }

        public int Ejecutar(int historiaId, int administradorId, string accion)
        {
            if (historiaId <= 0 || administradorId <= 0 || string.IsNullOrWhiteSpace(accion))
                throw new LogicaNegocioException("La historia, el administrador y la acción son obligatorios.");

            Historia historia = _historias.GetById(historiaId);
            Usuario usuario = _usuarios.GetById(administradorId);

            if (usuario is not Administrador administrador)
                throw new LogicaNegocioException("El usuario responsable debe ser un administrador.");

            AuditoriaHistoria auditoria = new AuditoriaHistoria(historia, administrador, accion.Trim());
            return _auditorias.Add(auditoria);
        }
    }
}
