using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ObligatorioP3.LogicaNegocio.Entidades;

namespace ObligatorioP3.Infraestructura.EF.AccesoDatos.Config
{
    public class AuditoriaHistoriaConfiguration : IEntityTypeConfiguration<AuditoriaHistoria>
    {
        public void Configure(EntityTypeBuilder<AuditoriaHistoria> builder)
        {
            builder.HasOne(a => a.Historia)
                .WithMany()
                .HasForeignKey("HistoriaId")
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired(false);

            builder.Property(a => a.HistoriaIdHistorico).IsRequired();

            builder.HasOne(a => a.Administrador)
                .WithMany()
                .HasForeignKey("AdministradorId")
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired();

            builder.Property(a => a.Accion).IsRequired();
            builder.Property(a => a.Fecha).IsRequired();
        }
    }
}
