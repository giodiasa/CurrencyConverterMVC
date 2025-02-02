using AutoMapper;
using CurrencyConverter.Application.Interfaces;
using CurrencyConverter.Core.Entities;
using CurrencyConverter.Core.Interfaces;
using CurrencyConverter.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyConverter.Application.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IMapper _mapper;

        public TransactionService(
            ITransactionRepository transactionRepository,
            IMapper mapper)
        {
            _transactionRepository = transactionRepository;
            _mapper = mapper;
        }

        public async Task AddTransactionAsync(CurrencyConversionRequestViewModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var transaction = _mapper.Map<Transaction>(model);
            await _transactionRepository.AddTransactionAsync(transaction);
        }

        public async Task<IEnumerable<TransactionViewModel>> GetAllTransactionsAsync()
        {
            var transactions = await _transactionRepository.GetAllTransactionsAsync();
            return _mapper.Map<IEnumerable<TransactionViewModel>>(transactions);
        }
    }
}
