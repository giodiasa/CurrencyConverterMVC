using AutoMapper;
using CurrencyConverter.Application.DTOs;
using CurrencyConverter.Application.Interfaces;
using CurrencyConverter.Core.Entities;
using CurrencyConverter.Core.Interfaces;
using CurrencyConverter.Core.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CurrencyConverter.Application.Services
{
    public class CurrencyConversionService : ICurrencyConversionService
    {
        private readonly IExchangeRateRepository _exchangeRateRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;

        public CurrencyConversionService(IExchangeRateRepository exchangeRateRepository, IMapper mapper, IConfiguration configuration)
        {
            _exchangeRateRepository = exchangeRateRepository;
            _mapper = mapper;
            _config = configuration;
        }

        public async Task<CurrencyConversionResponseViewModel> ConvertCurrencyAsync(CurrencyConversionRequestViewModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model), "Currency conversion model cannot be null");
            }

            if (model.FromCurrency.ToUpper() != "GEL" && model.ToCurrency.ToUpper() != "GEL")
            {
                throw new Exception("One of the currencies must be GEL");
            }

            ExchangeRate? rate = null;

            if (model.FromCurrency.ToUpper() == "GEL")
            {
                rate = await _exchangeRateRepository.GetLatestExchangeRateByCurrencyAsync(model.ToCurrency);
            }
            else
            {
                rate = await _exchangeRateRepository.GetLatestExchangeRateByCurrencyAsync(model.FromCurrency);
            }

            if (rate == null)
            {
                throw new Exception($"Exchange rate for {model.FromCurrency} or {model.ToCurrency} not found.");
            }

            decimal convertedAmount;

            if (model.FromCurrency.ToUpper() == "GEL")
            {
                convertedAmount = model.Amount / rate.Rate;
            }
            else
            {
                convertedAmount = model.Amount * rate.Rate;
            }
            var response = new CurrencyConversionResponseViewModel();
            response.ConvertedAmount = convertedAmount;
            response.ExchangeRateDate = rate.Date;
            response.Rate = rate.Rate;

            return response;
        }

        public async Task<ExchangeRateViewModel?> GetLatestExchangeRateAsync(string currency)
        {
            if (string.IsNullOrWhiteSpace(currency))
            {
                throw new ArgumentException("Currency cannot be null or empty", nameof(currency));
            }

            var exchangeRate = await _exchangeRateRepository.GetLatestExchangeRateByCurrencyAsync(currency);

            if (exchangeRate == null)
            {
                throw new Exception();
            }

            var exchangeRateViewModel = _mapper.Map<ExchangeRateViewModel>(exchangeRate);

            return exchangeRateViewModel;
        }
        public async Task FetchAndStoreLatestExchangeRatesAsync()
        {
            using var httpClient = new HttpClient();
            string apiUrl = _config["NBGSettings:Url"] ?? throw new Exception();

            HttpResponseMessage response = await httpClient.GetAsync(apiUrl);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Failed to fetch exchange rates from the API.");
            }

            string jsonResponse = await response.Content.ReadAsStringAsync();
            var exchangeRateData = JsonSerializer.Deserialize<List<NbgExchangeRateResponse>>(jsonResponse, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (exchangeRateData == null || exchangeRateData.Count == 0)
            {
                throw new Exception("No exchange rate data received from the API.");
            }

            var latestRates = exchangeRateData.First();
            var currenciesToStore = latestRates.Currencies
                .Where(c => new[] { "USD", "EUR", "GBP", "RUB" }.Contains(c.Code))
                .Select(c => new ExchangeRate
                {
                    Currency = c.Code,
                    Rate = c.Rate / c.Quantity,
                    Date = latestRates.Date
                })
                .ToList();

            foreach (var rate in currenciesToStore)
            {
                var existingRate = await _exchangeRateRepository.GetLatestExchangeRateByCurrencyAsync(rate.Currency);
                if (existingRate == null || existingRate.Date < rate.Date)
                {
                    await _exchangeRateRepository.AddExchangeRateAsync(rate);
                }
            }
        }
    }
}
