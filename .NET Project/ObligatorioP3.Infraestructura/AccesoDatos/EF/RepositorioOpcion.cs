using Microsoft.EntityFrameworkCore;
using ObligatorioP3.Infraestructura.AccesoDatos.EF.Excepciones;
using ObligatorioP3.Infraestructura.EF.AccesoDatos;
using ObligatorioP3.LogicaNegocio.Entidades;
using ObligatorioP3.LogicaNegocio.InterfacesRepositorio;

namespace ObligatorioP3.Infraestructura.AccesoDatos.EF
{
    public class RepositorioOpcion : IRepositorioOpcion
    {
        private ObligatorioP3Context _context;

        public RepositorioOpcion(ObligatorioP3Context context)
        {
            _context = context;
        }

        public int Add(Opcion obj)
        {
            if (obj == null)
                throw new AccesoDatosException("No se recibió opcion.");

            _context.Opciones.Add(obj);
            _context.SaveChanges();
            return obj.Id;
        }

        public void Delete(int id)
        {
            Opcion obj = GetById(id);
            _context.Opciones.Remove(obj);
            _context.SaveChanges();
        }

        public IEnumerable<Opcion> GetAll()
        {
            return _context.Opciones.Include(o => o.Destino)
                .OrderBy(obj => obj.Id)
                .ToList();
        }

        public Opcion GetById(int id)
        {
            Opcion? obj = _context.Opciones.Include(o => o.Destino)
                .FirstOrDefault(obj => obj.Id == id);

            if (obj == null)
                throw new NotFoundException("No se encontró opcion con Id " + id);

            return obj;
        }

        public bool ExisteOpcion(Opcion obj)
        {
            return obj != null && _context.Opciones.Any(x => x.Id == obj.Id);
        }

        public void Update(int id, Opcion obj)
        {
            if (obj == null)
                throw new AccesoDatosException("No se recibió opcion.");

            Opcion actual = GetById(id);
            if (obj.Id != id)
                throw new AccesoDatosException("El identificador no coincide.");

            _context.Entry(actual).State = EntityState.Detached;
            _context.Opciones.Update(obj);
            _context.SaveChanges();
        }
    }
}
