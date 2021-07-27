using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Cadastro.Domain.Models
{
    public partial class EmpresaModel : _Model
    {
        public EmpresaModel()
        {
            Filiais = new HashSet<FilialModel>();
            Socios = new HashSet<SocioModel>();
        }

        [DisplayName("Id")]
        public int EmpresaId { get; set; }

        [DisplayName("CGC")]
        [Required(ErrorMessage = "Campo obrigatório!")]
        public string Cgc { get; set; }

        [DisplayName("Empresa")]
        [Required(ErrorMessage = "Campo obrigatório!")]
        [StringLength(100, ErrorMessage = "Limite de caracteres excedido!")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Valor entre 1 e 2")]
        [Range(1, 2)]
        public int Tipo { get; set; }

        public string TipoEmpresa { get; set; }
        public int? EnderecoId { get; set; }

        public virtual EnderecoModel Endereco { get; set; }
        public virtual ICollection<FilialModel> Filiais { get; set; }
        public virtual ICollection<SocioModel> Socios { get; set; }
    }
}
