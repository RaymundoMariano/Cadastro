namespace Cadastro.API.Models.Aplicacao
{
    public partial class EnderecoPessoaModel
    {
        public int Id { get; set; }
        public int EnderecoId { get; set; }
        public int PessoaId { get; set; }

        public virtual EnderecoModel Endereco { get; set; }
        public virtual PessoaModel Pessoa { get; set; }
    }
}
