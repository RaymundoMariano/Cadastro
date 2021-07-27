using AutoMapper;
using Cadastro.Domain.Models;
using Cadastro.Domain.Entities;

namespace Cadastro.API.Mappers
{
    public class PessoaFisicaProfile : Profile
	{
		public PessoaFisicaProfile()
		{
			CreateMap<PessoaFisicaModel, PessoaFisica>().ReverseMap();
		}
	}
}
