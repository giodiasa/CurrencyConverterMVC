using CurrencyConverter.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyConverter.Infrastructure
{
    public class CurrencyConverterDbContext : DbContext
    {
        public CurrencyConverterDbContext(DbContextOptions<CurrencyConverterDbContext> options) : base(options) { }

        public DbSet<ExchangeRate> ExchangeRates { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
    }
}
