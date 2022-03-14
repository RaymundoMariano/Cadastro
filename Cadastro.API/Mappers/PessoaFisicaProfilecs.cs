using AutoMapper;
using Cadastro.Domain.Entities;
using Cadastro.Domain.Models.Aplicacao;

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
