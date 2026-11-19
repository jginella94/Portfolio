using Microsoft.EntityFrameworkCore;
using ObligatorioP3.Infraestructura.AccesoDatos.EF.Excepciones;
using ObligatorioP3.Infraestructura.EF.AccesoDatos;
using ObligatorioP3.LogicaNegocio.Entidades;
using ObligatorioP3.LogicaNegocio.InterfacesRepositorio;
using EFCore = Microsoft.EntityFrameworkCore.EF;

namespace ObligatorioP3.Infraestructura.AccesoDatos.EF
{
    public class RepositorioLectura : IRepositorioLectura
    {
        private readonly ObligatorioP3Context _context;

        public RepositorioLectura(ObligatorioP3Context context)
        {
            _context = context;
        }

        public int Add(Lectura obj)
        {
            if (obj is null)
                throw new AccesoDatosException("No se recibió lectura.");

            _context.Lecturas.Add(obj);
            _context.SaveChanges();
            return obj.Id;
        }

        public void Delete(int id)
        {
            Lectura obj = GetById(id);
            _context.Lecturas.Remove(obj);
            _context.SaveChanges();
        }

        public IEnumerable<Lectura> GetAll()
        {
            return _context.Lecturas
                .Include(l => l.Usuario)
                .Include(l => l.Historia)
                .Include(l => l.CapituloActual)
                .Include(l => l.FinalAlcanzado)
                .OrderBy(l => l.Id)
                .ToList();
        }

        public Lectura GetById(int id)
        {
            Lectura? obj = _context.Lecturas
                .Include(l => l.Usuario)
                .Include(l => l.Historia)
                .Include(l => l.CapituloActual)
                .Include(l => l.FinalAlcanzado)
                .FirstOrDefault(l => l.Id == id);

            if (obj is null)
                throw new NotFoundException("No se encontró lectura con Id " + id);

            CargarOpciones(obj);
            return obj;
        }

        public Lectura ObtenerParaUsuario(int lecturaId, int usuarioId)
        {
            Lectura? lectura = _context.Lecturas
                .Include(l => l.Historia)
                .Include(l => l.CapituloActual)
                .Include(l => l.FinalAlcanzado)
                .FirstOrDefault(l => l.Id == lecturaId && EFCore.Property<int>(l, "UsuarioId") == usuarioId);

            if (lectura is null)
                throw new NotFoundException("No se encontró la lectura del usuario.");

            CargarOpciones(lectura);
            return lectura;
        }

        private void CargarOpciones(Lectura lectura)
        {
            if (lectura.CapituloActual is CapituloIntermedio intermedio)
            {
                _context.Entry(intermedio)
                    .Collection(c => c.Opciones)
                    .Query()
                    .Include(o => o.Destino)
                    .Load();
            }
        }

        public IEnumerable<Lectura> ObtenerHistorial(int usuarioId, int mes, int anio)
        {
            DateTime desde = new DateTime(anio, mes, 1, 0, 0, 0, DateTimeKind.Utc);
            DateTime hasta = desde.AddMonths(1);

            return _context.Lecturas
                .AsNoTracking()
                .Include(l => l.Historia)
                .Include(l => l.FinalAlcanzado)
                .Where(l => EFCore.Property<int>(l, "UsuarioId") == usuarioId &&
                    l.FechaInicio >= desde && l.FechaInicio < hasta)
                .OrderByDescending(l => l.FechaInicio)
                .ThenByDescending(l => l.Id)
                .ToList();
        }

        public bool ExisteLectura(Lectura obj)
        {
            return obj != null && _context.Lecturas.Any(x => x.Id == obj.Id);
        }

        public void Update(int id, Lectura obj)
        {
            if (obj is null || obj.Id != id)
                throw new AccesoDatosException("La lectura o su identificador no son válidos.");

            Lectura actual = GetById(id);
            if (!ReferenceEquals(actual, obj))
                throw new AccesoDatosException("La lectura debe modificarse sobre la entidad recuperada del contexto.");

            _context.SaveChanges();
        }

        public void GuardarCambios()
        {
            _context.SaveChanges();
        }
    }
}
