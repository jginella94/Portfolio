using Microsoft.EntityFrameworkCore;
using ObligatorioP3.Infraestructura.AccesoDatos.EF.Excepciones;
using ObligatorioP3.Infraestructura.EF.AccesoDatos;
using ObligatorioP3.LogicaNegocio.Entidades;
using ObligatorioP3.LogicaNegocio.InterfacesRepositorio;

namespace ObligatorioP3.Infraestructura.AccesoDatos.EF
{
    public class RepositorioCategoria : IRepositorioCategoria
    {
        private readonly ObligatorioP3Context _context;

        public RepositorioCategoria(ObligatorioP3Context context)
        {
            _context = context;
        }

        public IEnumerable<Categoria> GetAll()
        {
            return _context.Categorias.OrderBy(c => c.Nombre.Value).ToList();
        }

        public Categoria GetById(int id)
        {
            return _context.Categorias.FirstOrDefault(c => c.Id == id)
                ?? throw new NotFoundException("No se encontró la categoría con Id " + id);
        }
    }
}
