using Cadastro.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Seguranca.Data.EFC.Tables
{
    public class PessoaTable : IEntityTypeConfiguration<Pessoa>
    {
        public void Configure(EntityTypeBuilder<Pessoa> builder)
        {
            builder.ToTable("Pessoa");

            builder.HasIndex(e => new { e.Nome, e.DataNascimento, e.NomeMae }, "IX_Pessoa")
                .IsUnique();

            builder.Property(e => e.DataNascimento).HasColumnType("datetime");

            builder.Property(e => e.Nome)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false);

            builder.Property(e => e.NomeMae)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false);
        }
    }
}
