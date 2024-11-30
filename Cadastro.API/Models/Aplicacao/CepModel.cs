using System.Collections.Generic;

namespace Cadastro.API.Models.Aplicacao
{
    public partial class CepModel
    {
        public CepModel()
        {
            Enderecos = [];
        }

        public string CEP { get; set; }
        public string Logradouro { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string Uf { get; set; }

        public virtual ICollection<EnderecoModel> Enderecos { get; set; }
    }
}
