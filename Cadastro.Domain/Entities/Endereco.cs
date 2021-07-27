using System.Collections.Generic;

namespace Cadastro.Domain.Entities
{
    public partial class Endereco : _Entity
    {
        public Endereco()
        {
            Empresas = new HashSet<Empresa>();
            EnderecoPessoas = new HashSet<EnderecoPessoa>();
        }

        public int EnderecoId { get; set; }
        public string CEP { get; set; }
        public string Logradouro { get; set; }
        public string Numero { get; set; }
        public string Complemento { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string Uf { get; set; }
        public int Tipo { get; set; }

        public virtual Cep CepNavigation { get; set; }
        public virtual ICollection<Empresa> Empresas { get; set; }
        public virtual ICollection<EnderecoPessoa> EnderecoPessoas { get; set; }
    }
}
