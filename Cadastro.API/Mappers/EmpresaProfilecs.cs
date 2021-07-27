using AutoMapper;
using Cadastro.Domain.Models;
using Cadastro.Domain.Entities;

namespace Cadastro.API.Mappers
{
    public class EmpresaProfile : Profile
	{
		public EmpresaProfile()
		{
			CreateMap<EmpresaModel, Empresa>().ReverseMap();
		}
	}
}
