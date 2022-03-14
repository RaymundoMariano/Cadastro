using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Cadastro.Domain.Models.Aplicacao
{
    public partial class PessoaModel : _Model
    {
        public PessoaModel()
        {
            EnderecoPessoas = new HashSet<EnderecoPessoaModel>();
            PessoaFisicas = new HashSet<PessoaFisicaModel>();
        }

        [DisplayName("Id")]
        public int PessoaId { get; set; }

        [DisplayName("Pessoa")]
        [Required(ErrorMessage = "Campo obrigatório!")]
        [StringLength(100, ErrorMessage = "Limite de caracteres excedido!")]
        public string Nome { get; set; }

        [DisplayName("Nascimento")]
        [DataType(DataType.Date)]
        public DateTime DataNascimento { get; set; }

        [DisplayName("Nome da Mãe")]
        [Required(ErrorMessage = "Campo obrigatório!")]
        [StringLength(100, ErrorMessage = "Limite de caracteres excedido!")]
        public string NomeMae { get; set; }

        [DisplayName("CPF")]
        public string Cpf { get; set; }

        public virtual ICollection<EnderecoPessoaModel> EnderecoPessoas { get; set; }
        public virtual ICollection<PessoaFisicaModel> PessoaFisicas { get; set; }
    }
}
