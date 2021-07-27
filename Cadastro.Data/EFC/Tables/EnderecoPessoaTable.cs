using Cadastro.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Seguranca.Data.EFC.Tables
{
    public class EnderecoPessoaTable : IEntityTypeConfiguration<EnderecoPessoa>
    {
        public void Configure(EntityTypeBuilder<EnderecoPessoa> builder)
        {
            builder.ToTable("EnderecoPessoa");

            builder.HasOne(d => d.Endereco)
                .WithMany(p => p.EnderecoPessoas)
                .HasForeignKey(d => d.EnderecoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EnderecoPessoa_Endereco");

            builder.HasOne(d => d.Pessoa)
                .WithMany(p => p.EnderecoPessoas)
                .HasForeignKey(d => d.PessoaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EnderecoPessoa_Pessoa");
        }
    }
}
