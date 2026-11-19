using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ObligatorioP3.LogicaNegocio.Entidades;

namespace ObligatorioP3.Infraestructura.EF.AccesoDatos.Config
{
    public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.OwnsOne(e => e.NombreCompleto, vo =>
            {
                vo.Property(v => v.Value).HasColumnName("NombreCompleto").HasMaxLength(150).IsRequired();
            });
            builder.Navigation(e => e.NombreCompleto).IsRequired();

            builder.OwnsOne(e => e.NombreUsuario, vo =>
            {
                vo.Property(v => v.Value).HasColumnName("NombreUsuario").HasMaxLength(50).IsRequired();
            });
            builder.Navigation(e => e.NombreUsuario).IsRequired();

            builder.OwnsOne(e => e.Password, vo =>
            {
                vo.Property(v => v.Value).HasColumnName("Password").HasMaxLength(256).IsRequired();
            });
            builder.Navigation(e => e.Password).IsRequired();

            builder.OwnsOne(e => e.Email, vo =>
            {
                vo.Property(v => v.Value).HasColumnName("Email").HasMaxLength(255).IsRequired();
            });
            builder.Navigation(e => e.Email).IsRequired();
        }
    }
}
