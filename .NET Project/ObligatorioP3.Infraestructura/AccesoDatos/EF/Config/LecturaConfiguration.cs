using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ObligatorioP3.LogicaNegocio.Entidades;

namespace ObligatorioP3.Infraestructura.EF.AccesoDatos.Config
{
    public class LecturaConfiguration : IEntityTypeConfiguration<Lectura>
    {
        public void Configure(EntityTypeBuilder<Lectura> builder)
        {
            builder.HasOne(l => l.Usuario)
                .WithMany()
                .HasForeignKey("UsuarioId")
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired();

            builder.HasOne(l => l.Historia)
                .WithMany()
                .HasForeignKey("HistoriaId")
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired();

            builder.HasOne(l => l.CapituloActual)
                .WithMany()
                .HasForeignKey("CapituloActualId")
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired();

            builder.HasOne(l => l.FinalAlcanzado)
                .WithMany()
                .HasForeignKey("FinalAlcanzadoId")
                .OnDelete(DeleteBehavior.NoAction);

            builder.Ignore(l => l.Completada);
        }
    }
}
