using Cadastro.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Seguranca.Data.EFC.Tables
{
    public class FilialTable : IEntityTypeConfiguration<Filial>
    {
        public void Configure(EntityTypeBuilder<Filial> builder)
        {
            builder.ToTable("Filial");

            builder.HasIndex(e => e.Cgc, "IX_Filial")
                .IsUnique();

            builder.Property(e => e.Cgc)
                .IsRequired()
                .HasMaxLength(14)
                .IsUnicode(false);

            builder.HasOne(d => d.Empresa)
                .WithMany(p => p.Filiais)
                .HasForeignKey(d => d.EmpresaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Filial_Empresa");
        }
    }
}
