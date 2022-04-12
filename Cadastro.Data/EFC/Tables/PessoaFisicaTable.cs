using Cadastro.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Seguranca.Data.EFC.Tables
{
    public class PessoaFisicaTable : IEntityTypeConfiguration<PessoaFisica>
    {
        public void Configure(EntityTypeBuilder<PessoaFisica> builder)
        {
            builder.ToTable("PessoaFisica");

            builder.HasIndex(e => new { e.Cpf }, "IX_PessoaFisica")
                .IsUnique();

            builder.Property(e => e.Cpf)
                .IsRequired()
                .HasMaxLength(11)
                .IsUnicode(false);

            builder.HasOne(d => d.Pessoa)
                .WithMany(p => p.PessoaFisicas)
                .HasForeignKey(d => d.PessoaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PessoaFisica_Pessoa");
        }
    }
}
