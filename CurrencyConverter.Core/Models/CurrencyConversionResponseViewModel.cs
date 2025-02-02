using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyConverter.Core.Models
{
    public class CurrencyConversionResponseViewModel
    {
        public decimal ConvertedAmount { get; set; }

        public DateTime ExchangeRateDate { get; set; }
        public decimal Rate { get; set; }
    }
}
