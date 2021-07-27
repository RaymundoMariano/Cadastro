namespace Cadastro.Domain.Models
{
    public partial class SocioModel
    {
        public int SocioId { get; set; }
        public int EmpresaId { get; set; }
        public string Cpf { get; set; }

        public virtual PessoaFisicaModel PessoaFisica { get; set; }
        public virtual EmpresaModel Empresa { get; set; }
    }
}
