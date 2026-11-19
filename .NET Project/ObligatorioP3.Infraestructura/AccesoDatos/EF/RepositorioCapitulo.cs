using Microsoft.EntityFrameworkCore;
using ObligatorioP3.Infraestructura.AccesoDatos.EF.Excepciones;
using ObligatorioP3.Infraestructura.EF.AccesoDatos;
using ObligatorioP3.LogicaNegocio.Entidades;
using ObligatorioP3.LogicaNegocio.InterfacesRepositorio;

namespace ObligatorioP3.Infraestructura.AccesoDatos.EF
{
    public class RepositorioCapitulo : IRepositorioCapitulo
    {
        private ObligatorioP3Context _context;

        public RepositorioCapitulo(ObligatorioP3Context context)
        {
            _context = context;
        }

        public int Add(Capitulo obj)
        {
            if (obj == null)
                throw new AccesoDatosException("No se recibió capitulo.");

            _context.Capitulos.Add(obj);
            _context.SaveChanges();
            return obj.Id;
        }

        public void Delete(int id)
        {
            Capitulo obj = GetById(id);
            _context.Capitulos.Remove(obj);
            _context.SaveChanges();
        }

        public IEnumerable<Capitulo> GetAll()
        {
            return _context.Capitulos
                .OrderBy(obj => obj.Id)
                .ToList();
        }

        public Capitulo GetById(int id)
        {
            Capitulo? obj = _context.Capitulos
                .FirstOrDefault(obj => obj.Id == id);

            if (obj == null)
                throw new NotFoundException("No se encontró capitulo con Id " + id);

            return obj;
        }

        public bool ExisteCapitulo(Capitulo obj)
        {
            return obj != null && _context.Capitulos.Any(x => x.Id == obj.Id);
        }

        public void Update(int id, Capitulo obj)
        {
            if (obj == null)
                throw new AccesoDatosException("No se recibió capitulo.");

            Capitulo actual = GetById(id);
            if (obj.Id != id)
                throw new AccesoDatosException("El identificador no coincide.");

            _context.Entry(actual).State = EntityState.Detached;
            _context.Capitulos.Update(obj);
            _context.SaveChanges();
        }
    }
}
