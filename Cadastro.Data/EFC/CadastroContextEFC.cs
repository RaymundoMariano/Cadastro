using Cadastro.Data.EFC.Tables;
using Cadastro.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Seguranca.Data.EFC.Tables;

namespace Cadastro.Data.EFC
{
    public partial class CadastroContextEFC : DbContext
    {
        public CadastroContextEFC()
        {
        }

        public CadastroContextEFC(DbContextOptions<CadastroContextEFC> options)
            : base(options)
        {
        }

        public virtual DbSet<Cep> Ceps { get; set; }
        public virtual DbSet<Endereco> Enderecos { get; set; }
        public virtual DbSet<EnderecoPessoa> EnderecoPessoas { get; set; }
        public virtual DbSet<Filial> Filiais { get; set; }
        public virtual DbSet<Pessoa> Pessoas { get; set; }
        public virtual DbSet<PessoaFisica> PessoaFisicas { get; set; }
        public virtual DbSet<Empresa> Empresas { get; set; }
        public virtual DbSet<Socio> Socios { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //if (!optionsBuilder.IsConfigured)
            //{
            //    optionsBuilder.UseSqlServer("Server=DESKTOP-S3R5UB7\\SQLEXPRESS;Database=DesBd_CADUN;Trusted_Connection=True;");
            //}
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Latin1_General_CI_AS");

            modelBuilder.ApplyConfiguration(new CepTable());
            modelBuilder.ApplyConfiguration(new EnderecoTable());
            modelBuilder.ApplyConfiguration(new EnderecoPessoaTable());
            modelBuilder.ApplyConfiguration(new FilialTable());
            modelBuilder.ApplyConfiguration(new PessoaTable());
            modelBuilder.ApplyConfiguration(new PessoaFisicaTable());
            modelBuilder.ApplyConfiguration(new EmpresaTable());
            modelBuilder.ApplyConfiguration(new SocioTable());

            OnModelCreatingPartial(modelBuilder);
        }
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
