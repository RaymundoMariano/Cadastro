namespace Cadastro.API.Models.Aplicacao
{
    public partial class FilialModel : _Model
    {
        public int FilialId { get; set; }
        public string Cgc { get; set; }
        public int EmpresaId { get; set; }
        public int EmpresaIdFilial { get; set; }
        public string Nome { get; set; }
        public string TipoEmpresa { get; set; }
        public virtual EmpresaModel Empresa { get; set; }
    }
}
