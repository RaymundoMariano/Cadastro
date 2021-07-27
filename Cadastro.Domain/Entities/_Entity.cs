using System.ComponentModel.DataAnnotations.Schema;

namespace Cadastro.Domain.Entities
{
    public class _Entity
    {   
        [NotMapped]
        public int Evento { get; set; }

        [NotMapped]
        public bool Selected { get; set; }
    }
}
