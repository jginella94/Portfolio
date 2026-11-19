using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ObligatorioP3.LogicaNegocio.Entidades;

namespace ObligatorioP3.Infraestructura.EF.AccesoDatos.Config
{
    public class CapituloFinalConfiguration : IEntityTypeConfiguration<CapituloFinal>
    {
        public void Configure(EntityTypeBuilder<CapituloFinal> builder)
        {
            builder.Property(c => c.VecesAlcanzado).IsRequired();
        }
    }
}
