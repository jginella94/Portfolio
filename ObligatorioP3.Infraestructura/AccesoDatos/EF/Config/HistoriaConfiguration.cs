using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ObligatorioP3.LogicaNegocio.Entidades;

namespace ObligatorioP3.Infraestructura.EF.AccesoDatos.Config
{
    public class HistoriaConfiguration : IEntityTypeConfiguration<Historia>
    {
        public void Configure(EntityTypeBuilder<Historia> builder)
        {
            builder.OwnsOne(e => e.Titulo, vo =>
            {
                vo.Property(v => v.Value).HasColumnName("Titulo").HasMaxLength(200).IsRequired();
            });
            builder.Navigation(e => e.Titulo).IsRequired();

            builder.OwnsOne(e => e.Sinopsis, vo =>
            {
                vo.Property(v => v.Value).HasColumnName("Sinopsis").HasMaxLength(1000).IsRequired();
            });
            builder.Navigation(e => e.Sinopsis).IsRequired();

            builder.Property(h => h.Estado).HasConversion<string>().IsRequired();

            builder.HasOne(h => h.CapituloInicial)
                .WithMany()
                .HasForeignKey("CapituloInicialId")
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired(false);

            builder.HasMany(h => h.Capitulos)
                .WithOne()
                .HasForeignKey("HistoriaId")
                .OnDelete(DeleteBehavior.NoAction);

            builder.Navigation(h => h.Capitulos)
                .HasField("_capitulos")
                .UsePropertyAccessMode(PropertyAccessMode.Field);

            builder.HasMany(h => h.Categorias)
                .WithMany()
                .UsingEntity<Dictionary<string, object>>(
                    "HistoriaCategoria",
                    r => r.HasOne<Categoria>().WithMany().HasForeignKey("CategoriaId").OnDelete(DeleteBehavior.NoAction),
                    l => l.HasOne<Historia>().WithMany().HasForeignKey("HistoriaId").OnDelete(DeleteBehavior.NoAction),
                    j => j.HasKey("HistoriaId", "CategoriaId"));

            builder.Navigation(h => h.Categorias)
                .HasField("_categorias")
                .UsePropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
