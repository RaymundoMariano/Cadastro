namespace Cadastro.Domain.Models.Aplicacao
{
    public partial class SocioModel
    {
        public int SocioId { get; set; }
        public int EmpresaId { get; set; }
        public int PessoaFisicaId { get; set; }

        public virtual PessoaFisicaModel PessoaFisica { get; set; }
        public virtual EmpresaModel Empresa { get; set; }
    }
}
