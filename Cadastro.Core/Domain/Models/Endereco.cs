using System.Collections.Generic;

namespace Cadastro.Core.Domain.Models
{
    public partial class Endereco
    {
        public Endereco()
        {
            EnderecoPessoa = new HashSet<EnderecoPessoa>();
            PessoaJuridica = new HashSet<PessoaJuridica>();
        }

        public int EnderecoId { get; set; }
        public long? CEP { get; set; }
        public string Logradouro { get; set; }
        public string Numero { get; set; }
        public string Complemento { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string Uf { get; set; }
        public int TpEndereco { get; set; }
        public virtual Cep CepNavigation { get; set; }
        public virtual ICollection<EnderecoPessoa> EnderecoPessoa { get; set; }
        public virtual ICollection<PessoaJuridica> PessoaJuridica { get; set; }
    }
}
