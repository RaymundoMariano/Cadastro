using System.Collections.Generic;

namespace Cadastro.Core.Domain.Models
{
    public partial class Cep
    {
        public Cep()
        {
            Endereco = new HashSet<Endereco>();
        }

        public long? CEP { get; set; }
        public string Logradouro { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string Uf { get; set; }

        public virtual ICollection<Endereco> Endereco { get; set; }
    }
}
