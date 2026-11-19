using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ObligatorioP3.LogicaNegocio.Entidades;

namespace ObligatorioP3.Infraestructura.EF.AccesoDatos.Config
{
    public class OpcionConfiguration : IEntityTypeConfiguration<Opcion>
    {
        public void Configure(EntityTypeBuilder<Opcion> builder)
        {
            builder.OwnsOne(e => e.Texto, vo =>
            {
                vo.Property(v => v.Value).HasColumnName("Texto").IsRequired();
            });
            builder.Navigation(e => e.Texto).IsRequired();

            builder.HasOne(o => o.Destino)
                .WithMany()
                .HasForeignKey("DestinoId")
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired();
        }
    }
}
