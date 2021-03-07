using System;

namespace Cadastro.Core.Domain.Models
{
    public partial class Socio
    {
        public long? Cgc { get; set; }
        public long? Cpf { get; set; }

        public virtual PessoaJuridica CgcNavigation { get; set; }
        public virtual PessoaFisica CpfNavigation { get; set; }
    }
}
