namespace Cadastro.Domain.Entities
{
    public partial class Socio : _Entity 
    {
        public int SocioId { get; set; }
        public int EmpresaId { get; set; }
        public string Cpf { get; set; }

        public virtual PessoaFisica PessoaFisica { get; set; }
        public virtual Empresa Empresa { get; set; }
    }
}
