using System;
using System.Collections.Generic;

namespace Cadastro.Core.Domain.Models
{
    public partial class PessoaJuridica
    {
        public PessoaJuridica()
        {
            FilialCgcMatrizNavigations = new HashSet<Filial>();
            FilialCgcNavigations = new HashSet<Filial>();
            Socio = new HashSet<Socio>();
        }

        public long? Cgc { get; set; }
        public string NmEmpresa { get; set; }
        public int TpEmpresa { get; set; }
        public int? EnderecoId { get; set; }

        public virtual Endereco Endereco { get; set; }
        public virtual ICollection<Filial> FilialCgcMatrizNavigations { get; set; }
        public virtual ICollection<Filial> FilialCgcNavigations { get; set; }
        public virtual ICollection<Socio> Socio { get; set; }
    }
}
