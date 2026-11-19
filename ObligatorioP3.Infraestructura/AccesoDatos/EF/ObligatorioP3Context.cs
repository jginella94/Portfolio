using Microsoft.EntityFrameworkCore;
using ObligatorioP3.LogicaNegocio.Entidades;

namespace ObligatorioP3.Infraestructura.EF.AccesoDatos
{
    public class ObligatorioP3Context : DbContext
    {
        public ObligatorioP3Context(DbContextOptions<ObligatorioP3Context> options) : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Administrador> Administradores { get; set; }
        public DbSet<Lector> Lectores { get; set; }
        public DbSet<Historia> Historias { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Capitulo> Capitulos { get; set; }
        public DbSet<CapituloIntermedio> CapitulosIntermedios { get; set; }
        public DbSet<CapituloFinal> CapitulosFinales { get; set; }
        public DbSet<Opcion> Opciones { get; set; }
        public DbSet<Lectura> Lecturas { get; set; }
        public DbSet<AuditoriaHistoria> AuditoriasHistoria { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ObligatorioP3Context).Assembly);
        }
    }
}
