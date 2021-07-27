using System.Collections.Generic;

namespace Cadastro.Domain.Entities
{
    public partial class PessoaFisica : _Entity
    {
        public PessoaFisica()
        {
            Socios = new HashSet<Socio>();
        }

        public string Cpf { get; set; }
        public int PessoaId { get; set; }

        public virtual Pessoa Pessoa { get; set; }
        public virtual ICollection<Socio> Socios { get; set; }
    }
}
