using AutoMapper;
using Cadastro.Domain.Models;
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
