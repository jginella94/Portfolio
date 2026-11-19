using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ObligatorioP3.LogicaNegocio.Entidades;

namespace ObligatorioP3.Infraestructura.EF.AccesoDatos.Config
{
    public class CapituloConfiguration : IEntityTypeConfiguration<Capitulo>
    {
        public void Configure(EntityTypeBuilder<Capitulo> builder)
        {
            builder.OwnsOne(e => e.Titulo, vo =>
            {
                vo.Property(v => v.Value).HasColumnName("Titulo").HasMaxLength(200).IsRequired();
            });
            builder.Navigation(e => e.Titulo).IsRequired();

            builder.OwnsOne(e => e.Texto, vo =>
            {
                vo.Property(v => v.Value).HasColumnName("Texto").IsRequired();
            });
            builder.Navigation(e => e.Texto).IsRequired();
        }
    }
}
