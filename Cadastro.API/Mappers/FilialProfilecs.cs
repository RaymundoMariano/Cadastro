using AutoMapper;
using Cadastro.Domain.Models;
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
