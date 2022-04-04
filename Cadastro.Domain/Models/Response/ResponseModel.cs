using System.Collections.Generic;

namespace Cadastro.Domain.Models.Response
{
    public class ResponseModel
    {
        public bool Succeeded { get; set; }
        public object ObjectRetorno { get; set; }        
        public int ObjectResult { get; set; }
        public List<string> Errors { get; set; }
    }
}
