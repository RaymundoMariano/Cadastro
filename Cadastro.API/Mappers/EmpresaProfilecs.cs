using AutoMapper;
using Cadastro.API.Models.Aplicacao;
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
