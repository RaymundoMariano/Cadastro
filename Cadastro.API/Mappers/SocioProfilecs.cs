using AutoMapper;
using Cadastro.Domain.Entities;
using Cadastro.Domain.Models;

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
