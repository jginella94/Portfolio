using Microsoft.EntityFrameworkCore;
using ObligatorioP3.Infraestructura.EF.AccesoDatos;
using ObligatorioP3.LogicaNegocio.Entidades;
using ObligatorioP3.LogicaNegocio.InterfacesRepositorio;
using EFCore = Microsoft.EntityFrameworkCore.EF;

namespace ObligatorioP3.Infraestructura.AccesoDatos.EF
{
    public class RepositorioRankingHistorias : IRepositorioRankingHistorias
    {
        private readonly ObligatorioP3Context _context;

        public RepositorioRankingHistorias(ObligatorioP3Context context)
        {
            _context = context;
        }

        public IEnumerable<(Historia Historia, int Cantidad)> ObtenerRanking()
        {
            var lecturasCompletadas = _context.Lecturas
                .AsNoTracking()
                .Where(l => l.FechaFinalizacion != null)
                .GroupBy(l => EFCore.Property<int>(l, "HistoriaId"))
                .Select(g => new
                {
                    HistoriaId = g.Key,
                    Cantidad = g.Count()
                });

            var ranking = _context.Historias
                .AsNoTracking()
                .Join(
                    lecturasCompletadas,
                    historia => historia.Id,
                    conteo => conteo.HistoriaId,
                    (historia, conteo) => new
                    {
                        Historia = historia,
                        Cantidad = conteo.Cantidad
                    })
                .OrderByDescending(r => r.Cantidad)
                .ThenBy(r => r.Historia.Id)
                .ToList();

            return ranking
                .Select(r => (r.Historia, r.Cantidad))
                .ToList();
        }
    }
}
