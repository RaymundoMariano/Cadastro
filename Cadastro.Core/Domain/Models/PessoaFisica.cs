 using System;
using System.Collections.Generic;

namespace Cadastro.Core.Domain.Models
{
    public partial class PessoaFisica
    {
        public PessoaFisica()
        {
            Socio = new HashSet<Socio>();
        }

        public long? Cpf { get; set; }
        public int PessoaId { get; set; }

        public virtual Pessoa Pessoa { get; set; }
        public virtual ICollection<Socio> Socio { get; set; }
    }
}
