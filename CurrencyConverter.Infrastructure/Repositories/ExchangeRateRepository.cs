using CurrencyConverter.Core.Entities;
using CurrencyConverter.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyConverter.Infrastructure.Repositories
{
    public class ExchangeRateRepository : IExchangeRateRepository
    {
        private readonly CurrencyConverterDbContext _dbContext;

        public ExchangeRateRepository(CurrencyConverterDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ExchangeRate?> GetLatestExchangeRateByCurrencyAsync(string currency)
        {
            return await _dbContext.ExchangeRates
                .Where(er => er.Currency == currency)
                .OrderByDescending(er => er.Date)
                .FirstOrDefaultAsync();
        }

        public async Task AddExchangeRateAsync(ExchangeRate exchangeRate)
        {
            await _dbContext.ExchangeRates.AddAsync(exchangeRate);
            await _dbContext.SaveChangesAsync();
        }
    }
}
