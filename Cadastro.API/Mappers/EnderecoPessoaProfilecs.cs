using AutoMapper;
using Cadastro.Domain.Models;
using Cadastro.Domain.Entities;

namespace Cadastro.API.Mappers
{
    public class EnderecoPessoaProfile : Profile
	{
		public EnderecoPessoaProfile()
		{
			CreateMap<EnderecoPessoaModel, EnderecoPessoa>().ReverseMap();
		}
	}
}
