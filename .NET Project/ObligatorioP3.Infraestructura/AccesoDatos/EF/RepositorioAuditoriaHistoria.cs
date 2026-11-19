using Microsoft.EntityFrameworkCore;
using ObligatorioP3.Infraestructura.AccesoDatos.EF.Excepciones;
using ObligatorioP3.Infraestructura.EF.AccesoDatos;
using ObligatorioP3.LogicaNegocio.Entidades;
using ObligatorioP3.LogicaNegocio.InterfacesRepositorio;
using EFCore = Microsoft.EntityFrameworkCore.EF;

namespace ObligatorioP3.Infraestructura.AccesoDatos.EF
{
    public class RepositorioAuditoriaHistoria : IRepositorioAuditoriaHistoria
    {
        private readonly ObligatorioP3Context _context;

        public RepositorioAuditoriaHistoria(ObligatorioP3Context context)
        {
            _context = context;
        }

        public int Add(AuditoriaHistoria obj)
        {
            if (obj is null)
                throw new AccesoDatosException("No se recibió auditoría.");

            _context.AuditoriasHistoria.Add(obj);
            _context.SaveChanges();
            return obj.Id;
        }

        public IEnumerable<AuditoriaHistoria> ObtenerPorFiltros(int? historiaId, int? administradorId)
        {
            IQueryable<AuditoriaHistoria> consulta = _context.AuditoriasHistoria
                .AsNoTracking()
                .Include(a => a.Historia)
                .Include(a => a.Administrador);

            if (historiaId.HasValue)
                consulta = consulta.Where(a => a.HistoriaIdHistorico == historiaId.Value);

            if (administradorId.HasValue)
                consulta = consulta.Where(a => EFCore.Property<int>(a, "AdministradorId") == administradorId.Value);

            return consulta.OrderByDescending(a => a.Fecha).ThenByDescending(a => a.Id).ToList();
        }
    }
}
