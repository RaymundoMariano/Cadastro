using AutoMapper;
using Cadastro.API.Models.Aplicacao;
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
