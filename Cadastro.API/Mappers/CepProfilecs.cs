﻿using AutoMapper;
using Cadastro.Domain.Entities;
using Cadastro.Domain.Models.Aplicacao;

namespace Cadastro.API.Mappers
{
    public class CepProfile : Profile
	{
		public CepProfile()
		{
			CreateMap<CepModel, Cep>().ReverseMap();
		}
	}
}
