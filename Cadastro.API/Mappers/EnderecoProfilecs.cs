using AutoMapper;
using Cadastro.API.Models.Aplicacao;
using Cadastro.Domain.Entities;

namespace Cadastro.API.Mappers
{
    public class EnderecoProfile : Profile
	{
		public EnderecoProfile()
		{
			CreateMap<EnderecoModel, Endereco>().ReverseMap();
		}
	}
}
