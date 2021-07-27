using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cadastro.Domain.Entities
{
    public partial class Pessoa : _Entity
    {
        public Pessoa()
        {
            EnderecoPessoas = new HashSet<EnderecoPessoa>();
            PessoaFisicas = new HashSet<PessoaFisica>();
        }

        public int PessoaId { get; set; }
        public string Nome { get; set; }
        public DateTime DataNascimento { get; set; }
        public string NomeMae { get; set; }

        [NotMapped]
        public string Cpf { get; set; }

        public virtual ICollection<EnderecoPessoa> EnderecoPessoas { get; set; }
        public virtual ICollection<PessoaFisica> PessoaFisicas { get; set; }
    }
}
