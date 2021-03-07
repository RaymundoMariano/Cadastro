using Cadastro.Core.Domain.Models;
using Cadastro.Core.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Cadastro.Core.Persistence.Contexts
{
    public partial class CadastroContext : DbContext, IUnitOfWork
    {
        public CadastroContext()
        {
        }

        public CadastroContext(DbContextOptions<CadastroContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Cep> Cep { get; set; }
        public virtual DbSet<Endereco> Endereco { get; set; }
        public virtual DbSet<EnderecoPessoa> EnderecoPessoa { get; set; }
        public virtual DbSet<Filial> Filial { get; set; }
        public virtual DbSet<Pessoa> Pessoa { get; set; }
        public virtual DbSet<PessoaFisica> PessoaFisica { get; set; }
        public virtual DbSet<PessoaJuridica> PessoaJuridica { get; set; }
        public virtual DbSet<Socio> Socio { get; set; }

        public Task<int> SaveChangesAsync()
        {
            return base.SaveChangesAsync();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //            if (!optionsBuilder.IsConfigured)
            //            {
            //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
            //                optionsBuilder.UseSqlServer("Server=DESKTOP-S3R5UB7\\SQLEXPRESS;Database=DesBd_CADUN;Trusted_Connection=True;");
            //            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Latin1_General_CI_AS");

            #region Cep
            modelBuilder.Entity<Cep>(entity =>
            {
                entity.HasKey(e => e.CEP);

                entity.ToTable("tb_cep");

                entity.Property(e => e.CEP)
                    .HasColumnType("numeric(8, 0)")
                    .HasColumnName("cep");

                entity.Property(e => e.Bairro)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("bairro");

                entity.Property(e => e.Cidade)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("cidade");

                entity.Property(e => e.Logradouro)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("logradouro");

                entity.Property(e => e.Uf)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("uf");
            });
            #endregion

            #region Endereco
            modelBuilder.Entity<Endereco>(entity =>
            {
                entity.HasKey(e => e.EnderecoId);

                entity.ToTable("tb_endereco");

                entity.Property(e => e.EnderecoId).HasColumnName("endereco_id");

                entity.Property(e => e.Bairro)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("bairro");

                entity.Property(e => e.CEP)
                    .HasColumnType("numeric(8, 0)")
                    .HasColumnName("cep");

                entity.Property(e => e.Cidade)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("cidade");

                entity.Property(e => e.Complemento)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("complemento");

                entity.Property(e => e.Logradouro)
                    .IsRequired()
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("logradouro");

                entity.Property(e => e.Numero)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("numero");

                entity.Property(e => e.TpEndereco)
                    .IsRequired()
                    //.HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("tp_endereco");

                entity.Property(e => e.Uf)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("uf");

                entity.HasOne(d => d.CepNavigation)
                    .WithMany(p => p.Endereco)
                    .HasForeignKey(d => d.CEP)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tb_endereco_tb_cep");
            });
            #endregion

            #region EnderecoPessoa
            modelBuilder.Entity<EnderecoPessoa>(entity =>
            {
                entity.HasKey(e => new { e.EnderecoId, e.PessoaId });

                entity.ToTable("tb_endereco_pessoa");

                entity.Property(e => e.EnderecoId).HasColumnName("endereco_id");

                entity.Property(e => e.PessoaId).HasColumnName("pessoa_id");

                entity.HasOne(d => d.Endereco)
                    .WithMany(p => p.EnderecoPessoa)
                    .HasForeignKey(d => d.EnderecoId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_tb_endereco_pessoa_tb_endereco");

                entity.HasOne(d => d.Pessoa)
                    .WithMany(p => p.EnderecoPessoa)
                    .HasForeignKey(d => d.PessoaId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_tb_endereco_pessoa_tb_pessoa");
            });
            #endregion

            #region Filial
            modelBuilder.Entity<Filial>(entity =>
            {
                entity.HasKey(e => new { e.Cgc, e.CgcMatriz });

                entity.ToTable("tb_filial");

                entity.Property(e => e.Cgc)
                    .HasColumnType("numeric(15, 0)")
                    .HasColumnName("cgc");

                entity.Property(e => e.CgcMatriz)
                    .HasColumnType("numeric(15, 0)")
                    .HasColumnName("cgc_matriz");

                entity.HasOne(d => d.CgcNavigation)
                    .WithMany(p => p.FilialCgcNavigations)
                    .HasForeignKey(d => d.Cgc)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tb_filial_tb_pessoa_juridica");

                entity.HasOne(d => d.CgcMatrizNavigation)
                    .WithMany(p => p.FilialCgcMatrizNavigations)
                    .HasForeignKey(d => d.CgcMatriz)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tb_filial_tb_pessoa_juridica1");
            });
            #endregion

            #region Pessoa
            modelBuilder.Entity<Pessoa>(entity =>
            {
                entity.HasKey(e => e.PessoaId);

                entity.ToTable("tb_pessoa");

                entity.HasIndex(e => new { e.NmPessoa, e.DtNascimento, e.NmMae }, "IX_tb_pessoa")
                    .IsUnique();

                entity.Property(e => e.PessoaId).HasColumnName("pessoa_id");

                entity.Property(e => e.DtNascimento)
                    .HasColumnType("datetime")
                    .HasColumnName("dt_nascimento");

                entity.Property(e => e.NmMae)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("nm_mae");

                entity.Property(e => e.NmPessoa)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("nm_pessoa");
            });
            #endregion

            #region PessoaFisica
            modelBuilder.Entity<PessoaFisica>(entity =>
            {
                entity.HasKey(e => e.Cpf);

                entity.ToTable("tb_pessoa_fisica");

                entity.HasIndex(e => e.PessoaId, "IX_tb_pessoa_fisica")
                    .IsUnique();

                entity.Property(e => e.Cpf)
                    .HasColumnType("numeric(11, 0)")
                    .HasColumnName("cpf");

                entity.Property(e => e.PessoaId).HasColumnName("pessoa_id");

                entity.HasOne(d => d.Pessoa)
                    .WithOne(p => p.PessoaFisica)
                    .HasForeignKey<PessoaFisica>(d => d.PessoaId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_tb_pessoa_fisica_tb_pessoa");
            });
            #endregion

            #region PessoaJuridica
            modelBuilder.Entity<PessoaJuridica>(entity =>
            {
                entity.HasKey(e => e.Cgc);

                entity.ToTable("tb_pessoa_juridica");

                entity.HasIndex(e => e.NmEmpresa, "IX_tb_pessoa_juridica")
                    .IsUnique();

                entity.Property(e => e.Cgc)
                    .HasColumnType("numeric(15, 0)")
                    .HasColumnName("cgc");

                entity.Property(e => e.EnderecoId).HasColumnName("endereco_id");

                entity.Property(e => e.NmEmpresa)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("nm_empresa");

                entity.Property(e => e.TpEmpresa)
                    .IsRequired()
                    .IsUnicode(false)
                    .HasColumnName("tp_empresa");

                entity.HasOne(d => d.Endereco)
                    .WithMany(p => p.PessoaJuridica)
                    .HasForeignKey(d => d.EnderecoId)
                    .HasConstraintName("FK_tb_pessoa_juridica_tb_endereco");
            });
            #endregion

            #region Socio
            modelBuilder.Entity<Socio>(entity =>
            {
                entity.HasKey(e => new { e.Cgc, e.Cpf });

                entity.ToTable("tb_socio");

                entity.HasIndex(e => new { e.Cgc, e.Cpf }, "IX_tb_socio")
                    .IsUnique();

                entity.Property(e => e.Cgc)
                    .HasColumnType("numeric(15, 0)")
                    .HasColumnName("cgc");

                entity.Property(e => e.Cpf)
                    .HasColumnType("numeric(11, 0)")
                    .HasColumnName("cpf");

                entity.HasOne(d => d.CgcNavigation)
                    .WithMany(p => p.Socio)
                    .HasForeignKey(d => d.Cgc)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_tb_socio_tb_pessoa_juridica1");

                entity.HasOne(d => d.CpfNavigation)
                    .WithMany(p => p.Socio)
                    .HasForeignKey(d => d.Cpf)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tb_socio_tb_pessoa_juridica");
            });
            #endregion

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
