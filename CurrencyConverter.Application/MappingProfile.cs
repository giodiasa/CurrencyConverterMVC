using AutoMapper;
using CurrencyConverter.Core.Entities;
using CurrencyConverter.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyConverter.Application
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Transaction, TransactionViewModel>().ReverseMap();
            CreateMap<ExchangeRate, ExchangeRateViewModel>().ReverseMap();

            CreateMap<CurrencyConversionRequestViewModel, Transaction>().ReverseMap();
            CreateMap<CurrencyConversionResponseViewModel, Transaction>().ReverseMap();
        }
    }
}
