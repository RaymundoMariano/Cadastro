using AutoMapper;
using Cadastro.API.Models.Aplicacao;
using Cadastro.Domain.Entities;

namespace Cadastro.API.Mappers
{
    public class FilialProfile : Profile
	{
		public FilialProfile()
		{
			CreateMap<FilialModel, Filial>().ReverseMap();
		}
	}
}
