using AutoMapper;
using Cadastro.API.Models.Aplicacao;
using Cadastro.Domain.Entities;

namespace Cadastro.API.Mappers
{
    public class CepProfile : Profile
	{
		public CepProfile()
		{
			CreateMap<CepModel, Cep>().ReverseMap();
		}
	}
}
