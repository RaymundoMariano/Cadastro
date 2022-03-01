using AutoMapper;
using Cadastro.Domain.Entities;
using Cadastro.Domain.Models;

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
