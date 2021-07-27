using AutoMapper;
using Cadastro.Domain.Models;
using Cadastro.Domain.Entities;

namespace Cadastro.API.Mappers
{
    public class PessoaProfile : Profile
	{
		public PessoaProfile()
		{
			CreateMap<PessoaModel, Pessoa>().ReverseMap();
		}
	}
}
