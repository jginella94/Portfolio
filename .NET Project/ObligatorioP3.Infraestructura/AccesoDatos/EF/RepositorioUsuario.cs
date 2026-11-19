using Microsoft.EntityFrameworkCore;
using ObligatorioP3.Infraestructura.AccesoDatos.EF.Excepciones;
using ObligatorioP3.Infraestructura.EF.AccesoDatos;
using ObligatorioP3.LogicaNegocio.Entidades;
using ObligatorioP3.LogicaNegocio.InterfacesRepositorio;

namespace ObligatorioP3.Infraestructura.AccesoDatos.EF
{
    public class RepositorioUsuario : IRepositorioUsuario
    {
        private ObligatorioP3Context _context;

        public RepositorioUsuario(ObligatorioP3Context context)
        {
            _context = context;
        }

        public int Add(Usuario obj)
        {
            if (obj == null)
                throw new AccesoDatosException("No se recibió usuario.");

            _context.Usuarios.Add(obj);
            _context.SaveChanges();
            return obj.Id;
        }

        public void Delete(int id)
        {
            Usuario obj = GetById(id);
            _context.Usuarios.Remove(obj);
            _context.SaveChanges();
        }

        public IEnumerable<Usuario> GetAll()
        {
            return _context.Usuarios
                .OrderBy(obj => obj.Id)
                .ToList();
        }

        public Usuario GetById(int id)
        {
            Usuario? obj = _context.Usuarios
                .FirstOrDefault(obj => obj.Id == id);

            if (obj == null)
                throw new NotFoundException("No se encontró usuario con Id " + id);

            return obj;
        }

        public Usuario? BuscarPorNombreUsuario(string nombreUsuario)
        {
            return _context.Usuarios.FirstOrDefault(u => u.NombreUsuario.Value == nombreUsuario);
        }

        public bool ExisteUsuario(Usuario obj)
        {
            return obj != null && _context.Usuarios.Any(x => x.Id == obj.Id);
        }

        public void Update(int id, Usuario obj)
        {
            if (obj == null)
                throw new AccesoDatosException("No se recibió usuario.");

            Usuario actual = GetById(id);
            if (obj.Id != id)
                throw new AccesoDatosException("El identificador no coincide.");

            _context.Entry(actual).State = EntityState.Detached;
            _context.Usuarios.Update(obj);
            _context.SaveChanges();
        }
    }
}
