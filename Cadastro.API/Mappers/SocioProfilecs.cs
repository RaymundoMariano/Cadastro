using AutoMapper;
using Cadastro.Domain.Models;
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
