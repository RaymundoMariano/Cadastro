using AutoMapper;
using Cadastro.API.Models.Aplicacao;
using Cadastro.Domain.Entities;

namespace Cadastro.API.Mappers
{
    public class SocioProfile : Profile
	{
		public SocioProfile()
		{
			CreateMap<SocioModel, Socio>().ReverseMap();
		}
	}
}
