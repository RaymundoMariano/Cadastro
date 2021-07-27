using Cadastro.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cadastro.Data.EFC.Tables
{
    public class CepTable : IEntityTypeConfiguration<Cep>
    {
        public void Configure(EntityTypeBuilder<Cep> builder)
        {
            builder.HasKey(e => e.CEP);

            builder.ToTable("Cep");

            builder.Property(e => e.CEP)
                .HasMaxLength(8)
                .HasColumnName("CEP")
                .IsUnicode(false);

            builder.Property(e => e.Bairro)
                .HasMaxLength(30)
                .IsUnicode(false);

            builder.Property(e => e.Cidade)
                .IsRequired()
                .HasMaxLength(30)
                .IsUnicode(false);

            builder.Property(e => e.Logradouro)
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.Uf)
                .IsRequired()
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("UF");
        }
    }
}
