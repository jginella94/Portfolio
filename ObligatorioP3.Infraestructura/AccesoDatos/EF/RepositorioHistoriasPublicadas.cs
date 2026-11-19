using Microsoft.EntityFrameworkCore;
using ObligatorioP3.Infraestructura.EF.AccesoDatos;
using ObligatorioP3.LogicaNegocio.Entidades;
using ObligatorioP3.LogicaNegocio.InterfacesRepositorio;

namespace ObligatorioP3.Infraestructura.AccesoDatos.EF
{
    public class RepositorioHistoriasPublicadas : IRepositorioHistoriasPublicadas
    {
        private readonly ObligatorioP3Context _context;

        public RepositorioHistoriasPublicadas(ObligatorioP3Context context)
        {
            _context = context;
        }

        public IEnumerable<Historia> ObtenerPublicadas(int? categoriaId)
        {
            IQueryable<Historia> consulta = _context.Historias
                .AsNoTracking()
                .Include(h => h.Categorias)
                .Where(h => h.Estado == EstadoHistoria.PUBLICADA);

            if (categoriaId.HasValue)
                consulta = consulta.Where(h => h.Categorias.Any(c => c.Id == categoriaId.Value));

            return consulta.OrderBy(h => h.Id).ToList();
        }
    }
}
