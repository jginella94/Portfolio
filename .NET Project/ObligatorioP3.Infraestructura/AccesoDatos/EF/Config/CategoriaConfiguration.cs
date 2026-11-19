using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ObligatorioP3.LogicaNegocio.Entidades;

namespace ObligatorioP3.Infraestructura.EF.AccesoDatos.Config
{
    public class CategoriaConfiguration : IEntityTypeConfiguration<Categoria>
    {
        public void Configure(EntityTypeBuilder<Categoria> builder)
        {
            builder.OwnsOne(e => e.Nombre, vo =>
            {
                vo.Property(v => v.Value).HasColumnName("Nombre").HasMaxLength(100).IsRequired();
            });
            builder.Navigation(e => e.Nombre).IsRequired();
        }
    }
}
