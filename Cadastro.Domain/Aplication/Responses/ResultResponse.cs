using System.Collections.Generic;

namespace Cadastro.Domain.Aplication.Responses
{
    public class ResultResponse
    {
        public bool Succeeded { get; set; }
        public object ObjectRetorno { get; set; }        
        public int ObjectResult { get; set; }
        public List<string> Errors { get; set; }
    }
}
