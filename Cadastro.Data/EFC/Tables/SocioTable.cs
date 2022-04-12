using Cadastro.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Seguranca.Data.EFC.Tables
{
    public class SocioTable : IEntityTypeConfiguration<Socio>
    {
        public void Configure(EntityTypeBuilder<Socio> builder)
        {
            builder.ToTable("Socio");

            builder.HasOne(d => d.PessoaFisica)
                .WithMany(p => p.Socios)
                .HasForeignKey(d => d.PessoaFisicaId)
                .HasConstraintName("FK_Socio_PessoaFisica");

            builder.HasOne(d => d.Empresa)
                .WithMany(p => p.Socios)
                .HasForeignKey(d => d.EmpresaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Socio_Empresa");
        }
    }
}
