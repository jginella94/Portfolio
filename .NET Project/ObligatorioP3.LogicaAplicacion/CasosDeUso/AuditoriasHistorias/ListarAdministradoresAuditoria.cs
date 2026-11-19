using ObligatorioP3.LogicaAplicacion.DTOs.AuditoriasHistorias;
using ObligatorioP3.LogicaNegocio.Entidades;
using ObligatorioP3.LogicaNegocio.InterfacesLogicaAplicacion;
using ObligatorioP3.LogicaNegocio.InterfacesRepositorio;

namespace ObligatorioP3.LogicaAplicacion.CasosDeUso.AuditoriasHistorias
{
    public class ListarAdministradoresAuditoria : IListarTodos<DtoAdministradorFiltroAuditoria>
    {
        private readonly IRepositorioUsuario _repositorio;

        public ListarAdministradoresAuditoria(IRepositorioUsuario repositorio)
        {
            _repositorio = repositorio;
        }

        public IEnumerable<DtoAdministradorFiltroAuditoria> Ejecutar()
        {
            return _repositorio.GetAll()
                .OfType<Administrador>()
                .Select(a => new DtoAdministradorFiltroAuditoria(a.Id, a.NombreCompleto.Value))
                .OrderBy(a => a.NombreCompleto)
                .ToList();
        }
    }
}
