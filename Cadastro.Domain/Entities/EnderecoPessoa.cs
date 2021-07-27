namespace Cadastro.Domain.Entities
{
    public partial class EnderecoPessoa : _Entity
    {
        public int Id { get; set; }
        public int EnderecoId { get; set; }
        public int PessoaId { get; set; }

        public virtual Endereco Endereco { get; set; }
        public virtual Pessoa Pessoa { get; set; }
    }
}
