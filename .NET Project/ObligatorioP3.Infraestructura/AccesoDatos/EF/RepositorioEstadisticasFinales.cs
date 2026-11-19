using Microsoft.EntityFrameworkCore;
using ObligatorioP3.Infraestructura.EF.AccesoDatos;
using ObligatorioP3.LogicaNegocio.Entidades;
using ObligatorioP3.LogicaNegocio.InterfacesRepositorio;
using EFCore = Microsoft.EntityFrameworkCore.EF;

namespace ObligatorioP3.Infraestructura.AccesoDatos.EF
{
    public class RepositorioEstadisticasFinales : IRepositorioEstadisticasFinales
    {
        private readonly ObligatorioP3Context _context;

        public RepositorioEstadisticasFinales(ObligatorioP3Context context)
        {
            _context = context;
        }

        public Historia? ObtenerHistoria(int historiaId)
        {
            return _context.Historias
                .AsNoTracking()
                .FirstOrDefault(h => h.Id == historiaId);
        }

        public IEnumerable<Historia> ObtenerHistorias()
        {
            return _context.Historias
                .AsNoTracking()
                .OrderBy(h => h.Id)
                .ToList();
        }

        public IEnumerable<(CapituloFinal Final, int Cantidad)> ObtenerFrecuenciaFinales(int historiaId)
        {
            var consulta = _context.CapitulosFinales
                .AsNoTracking()
                .Where(f => EFCore.Property<int>(f, "HistoriaId") == historiaId)
                .Select(f => new
                {
                    Final = f,
                    Cantidad = _context.Lecturas.Count(l =>
                        EFCore.Property<int>(l, "HistoriaId") == historiaId &&
                        EFCore.Property<int?>(l, "FinalAlcanzadoId") == f.Id &&
                        l.FechaFinalizacion != null)
                })
                .OrderByDescending(r => r.Cantidad)
                .ThenBy(r => r.Final.Id)
                .ToList();

            return consulta.Select(r => (r.Final, r.Cantidad)).ToList();
        }
    }
}
