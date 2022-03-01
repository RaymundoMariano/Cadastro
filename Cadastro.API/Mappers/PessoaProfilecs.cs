using AutoMapper;
using Cadastro.Domain.Entities;
using Cadastro.Domain.Models;

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
