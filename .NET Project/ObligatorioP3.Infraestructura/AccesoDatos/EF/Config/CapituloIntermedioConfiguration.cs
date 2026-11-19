using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ObligatorioP3.LogicaNegocio.Entidades;

namespace ObligatorioP3.Infraestructura.EF.AccesoDatos.Config
{
    public class CapituloIntermedioConfiguration : IEntityTypeConfiguration<CapituloIntermedio>
    {
        public void Configure(EntityTypeBuilder<CapituloIntermedio> builder)
        {
            builder.HasMany(c => c.Opciones)
                .WithOne()
                .HasForeignKey("CapituloIntermedioId")
                .OnDelete(DeleteBehavior.NoAction);

            builder.Navigation(c => c.Opciones)
                .UsePropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
