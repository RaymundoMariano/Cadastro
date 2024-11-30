using System.Collections.Generic;
using System.ComponentModel;

namespace Cadastro.API.Models.Aplicacao
{
    public partial class PessoaFisicaModel
    {
        public PessoaFisicaModel()
        {
            Socios = [];
        }

        [DisplayName("Id")]
        public int PessoaFisicaId { get; set; }
        public string Cpf { get; set; }
        public int PessoaId { get; set; }

        public virtual PessoaModel Pessoa { get; set; }
        public virtual ICollection<SocioModel> Socios { get; set; }
    }
}
