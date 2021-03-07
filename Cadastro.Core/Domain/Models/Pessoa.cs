using System;
using System.Collections.Generic;

namespace Cadastro.Core.Domain.Models
{
    public partial class Pessoa
    {
        public Pessoa()
        {
            EnderecoPessoa = new HashSet<EnderecoPessoa>();
        }

        public int PessoaId { get; set; }
        public string NmPessoa { get; set; }
        public DateTime DtNascimento { get; set; }
        public string NmMae { get; set; }

        public virtual PessoaFisica PessoaFisica { get; set; }
        public virtual ICollection<EnderecoPessoa> EnderecoPessoa { get; set; }
    }
}
