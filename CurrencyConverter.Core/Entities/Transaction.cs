using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyConverter.Core.Entities
{
    public class Transaction
    {
        public int Id { get; set; }
        public string ClientName { get; set; } = string.Empty;
        public string PersonalNumber { get; set; } = string.Empty;
        public string FromCurrency { get; set; } = string.Empty;
        public string ToCurrency { get; set; } = string.Empty;
        public decimal Rate { get; set; }
        public DateTime Date { get; set; }
    }
}
