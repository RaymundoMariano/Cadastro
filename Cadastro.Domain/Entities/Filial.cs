namespace Cadastro.Domain.Entities
{
    public partial class Filial : _Entity
    {
        public int FilialId { get; set; }
        public string Cgc { get; set; }
        public int EmpresaId { get; set; }

        public virtual Empresa Empresa { get; set; }
    }
}
