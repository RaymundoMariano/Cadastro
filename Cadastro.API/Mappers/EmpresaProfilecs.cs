using AutoMapper;
using Cadastro.Domain.Entities;
using Cadastro.Domain.Models;

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
