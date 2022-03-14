using AutoMapper;
using Cadastro.Domain.Entities;
using Cadastro.Domain.Models.Aplicacao;

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
