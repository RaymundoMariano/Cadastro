namespace Cadastro.Core.Domain.Models
{
    public partial class EnderecoPessoa
    {
        public int EnderecoId { get; set; }
        public int PessoaId { get; set; }

        public virtual Endereco Endereco { get; set; }
        public virtual Pessoa Pessoa { get; set; }
    }
}
