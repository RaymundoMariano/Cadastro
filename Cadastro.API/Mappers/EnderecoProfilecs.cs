using AutoMapper;
using Cadastro.Domain.Models;
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
