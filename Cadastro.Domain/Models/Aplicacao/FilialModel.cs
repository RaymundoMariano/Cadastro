namespace Cadastro.Domain.Models.Aplicacao
{
    public partial class FilialModel : _Model
    {
        public int FilialId { get; set; }
        public string Cgc { get; set; }
        public int EmpresaId { get; set; }

        public virtual EmpresaModel Empresa { get; set; }
    }
}
