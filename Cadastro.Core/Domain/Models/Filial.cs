using System;

namespace Cadastro.Core.Domain.Models
{
    public partial class Filial
    {
        public long? Cgc { get; set; }
        public long? CgcMatriz { get; set; }

        public virtual PessoaJuridica CgcMatrizNavigation { get; set; }
        public virtual PessoaJuridica CgcNavigation { get; set; }
    }
}
