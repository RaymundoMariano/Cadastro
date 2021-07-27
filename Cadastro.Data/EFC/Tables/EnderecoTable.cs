using Cadastro.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Seguranca.Data.EFC.Tables
{
    public class EnderecoTable : IEntityTypeConfiguration<Endereco>
    {
        public void Configure(EntityTypeBuilder<Endereco> builder)
        {
            builder.ToTable("Endereco");

            builder.Property(e => e.Bairro)
                .IsRequired()
                .HasMaxLength(30)
                .IsUnicode(false);

            builder.Property(e => e.CEP)
                .IsRequired()
                .HasMaxLength(8)
                .IsUnicode(false);

            builder.Property(e => e.Cidade)
                .IsRequired()
                .HasMaxLength(30)
                .IsUnicode(false);

            builder.Property(e => e.Complemento)
                .HasMaxLength(15)
                .IsUnicode(false);

            builder.Property(e => e.Logradouro)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false);

            builder.Property(e => e.Numero)
                .HasMaxLength(5)
                .IsUnicode(false);

            builder.Property(e => e.Uf)
                .IsRequired()
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("UF");

            builder.HasOne(d => d.CepNavigation)
                .WithMany(p => p.Enderecos)
                .HasForeignKey(d => d.CEP)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Endereco_Cep");
        }
    }
}
