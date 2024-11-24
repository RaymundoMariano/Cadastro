using System.Collections.Generic;

namespace Cadastro.API.Models.Response
{
    public class ResponseModel
    {
        public bool Succeeded { get; set; }
        public List<string> Errors { get; set; }
        public object ObjectRetorno { get; set; }        
    }
}
