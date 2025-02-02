using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyConverter.Application.DTOs
{
    public class NbgExchangeRateResponse
    {
        public DateTime Date { get; set; }
        public List<NbgCurrencyRate> Currencies { get; set; } = new();
    }
}
