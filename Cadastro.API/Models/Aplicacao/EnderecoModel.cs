using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Cadastro.API.Models.Aplicacao
{
    public partial class EnderecoModel : _Model
    {
        public EnderecoModel()
        {
            Empresas = [];
            EnderecoPessoas = [];
        }

        [DisplayName("Id")]
        public int EnderecoId { get; set; }

        [Required(ErrorMessage = "Campo obrigatório!")]
        public string CEP { get; set; }

        public string Logradouro { get; set; }
        public string Numero { get; set; }
        public string Complemento { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string Uf { get; set; }

        [Required(ErrorMessage = "Campo obrigatório!")]
        public int Tipo { get; set; }
        public string TipoEndereco { get; set; }

        public virtual CepModel CepNavigation { get; set; }
        public virtual ICollection<EmpresaModel> Empresas { get; set; }
        public virtual ICollection<EnderecoPessoaModel> EnderecoPessoas { get; set; }
    }
}
