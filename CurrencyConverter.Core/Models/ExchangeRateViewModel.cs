using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyConverter.Core.Models
{
    public class ExchangeRateViewModel
    {
        public string Currency { get; set; } = string.Empty;
        public decimal Rate { get; set; }
        public DateTime Date { get; set; }
    }
}
