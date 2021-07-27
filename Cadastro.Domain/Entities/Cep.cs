using System.Collections.Generic;

namespace Cadastro.Domain.Entities
{
    public partial class Cep : _Entity
    {
        public Cep()
        {
            Enderecos = new HashSet<Endereco>();
        }

        public string CEP { get; set; }
        public string Logradouro { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string Uf { get; set; }

        public virtual ICollection<Endereco> Enderecos { get; set; }
    }
}
