using Cadastro.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Seguranca.Data.EFC.Tables
{
    public class EmpresaTable : IEntityTypeConfiguration<Empresa>
    {
        public void Configure(EntityTypeBuilder<Empresa> builder)
        {
            builder.ToTable("Empresa");

            builder.HasIndex(e => e.Cgc, "IX_Empresa");

            builder.HasIndex(e => e.Nome, "IX_Empresa_1");

            builder.Property(e => e.Cgc)
                .IsRequired()
                .HasMaxLength(14)
                .IsUnicode(false);

            builder.Property(e => e.Nome)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false);

            builder.HasOne(d => d.Endereco)
                .WithMany(p => p.Empresas)
                .HasForeignKey(d => d.EnderecoId)
                .HasConstraintName("FK_Empresa_Endereco");
        }
    }
}
