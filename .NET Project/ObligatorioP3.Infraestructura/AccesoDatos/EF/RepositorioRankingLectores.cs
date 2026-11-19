using Microsoft.EntityFrameworkCore;
using ObligatorioP3.Infraestructura.EF.AccesoDatos;
using ObligatorioP3.LogicaNegocio.Entidades;
using ObligatorioP3.LogicaNegocio.InterfacesRepositorio;
using EFCore = Microsoft.EntityFrameworkCore.EF;

namespace ObligatorioP3.Infraestructura.AccesoDatos.EF
{
    public class RepositorioRankingLectores : IRepositorioRankingLectores
    {
        private readonly ObligatorioP3Context _context;

        public RepositorioRankingLectores(ObligatorioP3Context context)
        {
            _context = context;
        }

        public IEnumerable<(Lector Lector, int Cantidad)> ObtenerRanking()
        {
            var historiasCompletadas = _context.Lecturas
                .AsNoTracking()
                .Where(l => l.FechaFinalizacion != null)
                .GroupBy(l => EFCore.Property<int>(l, "UsuarioId"))
                .Select(g => new
                {
                    LectorId = g.Key,
                    Cantidad = g.Select(l => EFCore.Property<int>(l, "HistoriaId"))
                        .Distinct()
                        .Count()
                });

            var ranking = _context.Lectores
                .AsNoTracking()
                .Join(
                    historiasCompletadas,
                    lector => lector.Id,
                    conteo => conteo.LectorId,
                    (lector, conteo) => new
                    {
                        Lector = lector,
                        Cantidad = conteo.Cantidad
                    })
                .OrderByDescending(r => r.Cantidad)
                .ThenBy(r => r.Lector.Id)
                .ToList();

            return ranking
                .Select(r => (r.Lector, r.Cantidad))
                .ToList();
        }
    }
}
