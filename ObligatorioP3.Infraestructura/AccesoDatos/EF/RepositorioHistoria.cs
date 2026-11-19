using Microsoft.EntityFrameworkCore;
using ObligatorioP3.Infraestructura.AccesoDatos.EF.Excepciones;
using ObligatorioP3.Infraestructura.EF.AccesoDatos;
using ObligatorioP3.LogicaNegocio.Entidades;
using ObligatorioP3.LogicaNegocio.Excepciones;
using ObligatorioP3.LogicaNegocio.InterfacesRepositorio;
using EFCore = Microsoft.EntityFrameworkCore.EF;

namespace ObligatorioP3.Infraestructura.AccesoDatos.EF
{
    public class RepositorioHistoria : IRepositorioHistoria, IRepositorioBajaHistoriaAuditada
    {
        private ObligatorioP3Context _context;

        public RepositorioHistoria(ObligatorioP3Context context)
        {
            _context = context;
        }

        public int Add(Historia obj)
        {
            if (obj is null)
                throw new AccesoDatosException("No se recibió historia.");

            obj.Validar();
            CapituloIntermedio inicial = obj.CapituloInicial!;

            using var transaccion = _context.Database.BeginTransaction();

            try
            {
                _context.Historias.Add(obj);
                _context.Entry(inicial).State = EntityState.Detached;
                _context.Entry(obj).Reference(h => h.CapituloInicial).CurrentValue = null;

                bool detectarCambios = _context.ChangeTracker.AutoDetectChangesEnabled;
                try
                {
                    _context.ChangeTracker.AutoDetectChangesEnabled = false;
                    _context.SaveChanges();
                }
                finally
                {
                    _context.ChangeTracker.AutoDetectChangesEnabled = detectarCambios;
                }

                _context.Entry(obj).Reference(h => h.CapituloInicial).CurrentValue = inicial;
                _context.Entry(inicial).State = EntityState.Added;
                _context.ChangeTracker.DetectChanges();
                _context.Entry(inicial).Property("HistoriaId").CurrentValue = obj.Id;
                _context.SaveChanges();

                transaccion.Commit();
                return obj.Id;
            }
            catch
            {
                transaccion.Rollback();
                _context.Entry(obj).Reference(h => h.CapituloInicial).CurrentValue = inicial;
                _context.ChangeTracker.Clear();
                throw;
            }
        }

        public void Delete(int id)
        {
            using var transaccion = _context.Database.BeginTransaction();

            try
            {
                EliminarHistoria(id);
                transaccion.Commit();
            }
            catch
            {
                transaccion.Rollback();
                _context.ChangeTracker.Clear();
                throw;
            }
        }

        public void DeleteConAuditoria(int historiaId, int administradorId)
        {
            using var transaccion = _context.Database.BeginTransaction();

            try
            {
                Historia? historia = _context.Historias.FirstOrDefault(h => h.Id == historiaId);

                if (historia is null)
                    throw new NotFoundException("No se encontró historia con Id " + historiaId);

                if (_context.Lecturas.Any(l => EFCore.Property<int>(l, "HistoriaId") == historiaId))
                    throw new HistoriaInvalidaException("No se puede eliminar una historia con lecturas asociadas.");

                Administrador? administrador = _context.Administradores.FirstOrDefault(a => a.Id == administradorId);

                if (administrador is null)
                    throw new NotFoundException("No se encontró administrador con Id " + administradorId);

                _context.AuditoriasHistoria.Add(new AuditoriaHistoria(historia, administrador, "ELIMINACION_HISTORIA"));
                _context.SaveChanges();
                _context.ChangeTracker.Clear();

                EliminarHistoria(historiaId);
                transaccion.Commit();
            }
            catch
            {
                transaccion.Rollback();
                _context.ChangeTracker.Clear();
                throw;
            }
        }

        private void EliminarHistoria(int id)
        {
            Historia? historia = _context.Historias.FirstOrDefault(h => h.Id == id);

            if (historia is null)
                throw new NotFoundException("No se encontró historia con Id " + id);

            if (_context.Lecturas.Any(l => EFCore.Property<int>(l, "HistoriaId") == id))
                throw new HistoriaInvalidaException("No se puede eliminar una historia con lecturas asociadas.");

            _context.AuditoriasHistoria
                .Where(a => EFCore.Property<int?>(a, "HistoriaId") == id)
                .ExecuteUpdate(setters => setters
                    .SetProperty(a => EFCore.Property<int?>(a, "HistoriaId"), (int?)null));

            int[] capitulosIds = _context.Capitulos
                .Where(c => EFCore.Property<int?>(c, "HistoriaId") == id)
                .Select(c => c.Id)
                .ToArray();

            if (capitulosIds.Length > 0)
            {
                _context.Opciones
                    .Where(o => capitulosIds.Contains(EFCore.Property<int>(o, "DestinoId")) ||
                                capitulosIds.Contains(EFCore.Property<int?>(o, "CapituloIntermedioId") ?? -1))
                    .ExecuteDelete();
            }

            _context.Set<Dictionary<string, object>>("HistoriaCategoria")
                .Where(hc => EFCore.Property<int>(hc, "HistoriaId") == id)
                .ExecuteDelete();

            _context.Entry(historia).Property("CapituloInicialId").CurrentValue = null;
            _context.SaveChanges();

            _context.Capitulos
                .Where(c => EFCore.Property<int?>(c, "HistoriaId") == id)
                .ExecuteDelete();

            _context.Historias.Remove(historia);
            _context.SaveChanges();
        }

        public IEnumerable<Historia> GetAll()
        {
            List<Historia> historias = _context.Historias
                .Include(h => h.Categorias)
                .Include(h => h.Capitulos)
                .Include(h => h.CapituloInicial)
                .OrderBy(h => h.Id)
                .ToList();

            foreach (Historia historia in historias)
                CargarOpciones(historia);

            return historias;
        }

        public Historia GetById(int id)
        {
            Historia? obj = _context.Historias
                .Include(h => h.Categorias)
                .Include(h => h.Capitulos)
                .Include(h => h.CapituloInicial)
                .FirstOrDefault(h => h.Id == id);

            if (obj is null)
                throw new NotFoundException("No se encontró historia con Id " + id);

            CargarOpciones(obj);

            return obj;
        }

        private void CargarOpciones(Historia historia)
        {
            foreach (CapituloIntermedio intermedio in historia.Capitulos.OfType<CapituloIntermedio>())
            {
                _context.Entry(intermedio)
                    .Collection(c => c.Opciones)
                    .Query()
                    .Include(o => o.Destino)
                    .Load();
            }
        }

        public bool ExisteHistoria(Historia obj)
        {
            return obj != null && _context.Historias.Any(x => x.Id == obj.Id);
        }

        public void Update(int id, Historia obj)
        {
            if (obj is null || obj.Id != id)
                throw new AccesoDatosException("La historia o su identificador no son válidos.");

            Historia actual = GetById(id);
            if (!ReferenceEquals(actual, obj))
                throw new AccesoDatosException("La historia debe modificarse sobre la entidad recuperada del contexto.");

            _context.SaveChanges();
        }
    }
}
