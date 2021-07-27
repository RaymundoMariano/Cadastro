using System.Collections.Generic;

namespace Cadastro.Domain.Entities
{
    public partial class Empresa : _Entity
    {
        public Empresa()
        {
            Filiais = new HashSet<Filial>();
            Socios = new HashSet<Socio>();
        }

        public int EmpresaId { get; set; }
        public string Cgc { get; set; }
        public string Nome { get; set; }
        public int Tipo { get; set; }
        public int? EnderecoId { get; set; }

        public virtual Endereco Endereco { get; set; }
        public virtual ICollection<Filial> Filiais { get; set; }
        public virtual ICollection<Socio> Socios { get; set; }
    }
}
