using AutoMapper;
using Cadastro.Domain.Entities;
using Cadastro.Domain.Models.Aplicacao;

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
