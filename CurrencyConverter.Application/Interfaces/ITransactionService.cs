using CurrencyConverter.Core.Entities;
using CurrencyConverter.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyConverter.Application.Interfaces
{
    public interface ITransactionService
    {
        Task<IEnumerable<TransactionViewModel>> GetAllTransactionsAsync();
        Task AddTransactionAsync(CurrencyConversionRequestViewModel model);
    }
}
