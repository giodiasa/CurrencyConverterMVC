using CurrencyConverter.Core.Entities;
using CurrencyConverter.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyConverter.Application.Interfaces
{
    public interface ICurrencyConversionService
    {
        Task<CurrencyConversionResponseViewModel> ConvertCurrencyAsync(CurrencyConversionRequestViewModel model);
        Task<ExchangeRateViewModel?> GetLatestExchangeRateAsync(string currency);
        Task FetchAndStoreLatestExchangeRatesAsync();
    }
}
