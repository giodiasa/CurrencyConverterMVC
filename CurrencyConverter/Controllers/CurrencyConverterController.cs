using Microsoft.AspNetCore.Mvc;
using CurrencyConverter.Core.Models;
using CurrencyConverter.Application.Interfaces;
using X.PagedList.Extensions;

namespace CurrencyConverter.Controllers
{
    public class CurrencyConversionController : Controller
    {
        private readonly ITransactionService _transactionService;
        private readonly ICurrencyConversionService _currencyConversionService;

        public CurrencyConversionController(
            ITransactionService transactionService,
            ICurrencyConversionService currencyConversionService)
        {
            _transactionService = transactionService;
            _currencyConversionService = currencyConversionService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int? page)
        {
            int pageSize = 10; 
            int pageNumber = (page ?? 1); 

            var transactions = await _transactionService.GetAllTransactionsAsync();
            var paginatedTransactions = transactions.ToPagedList(pageNumber, pageSize);
            ViewBag.Transactions = paginatedTransactions;
            return View(new CurrencyConversionRequestViewModel());
        }
        [HttpPost]
        public async Task<IActionResult> Index(CurrencyConversionRequestViewModel model, int? page)
        {
            int pageSize = 10;
            int pageNumber = (page ?? 1);

            var transactions = await _transactionService.GetAllTransactionsAsync();
            var paginatedTransactions = transactions.ToPagedList(pageNumber, pageSize);

            if (ModelState.IsValid)
            {
                var result =  await _currencyConversionService.ConvertCurrencyAsync(model);
                model.Rate = result.Rate;
                await _transactionService.AddTransactionAsync(model);
            }
            ViewBag.Transactions = paginatedTransactions;
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateExchangeRates()
        {
            await _currencyConversionService.FetchAndStoreLatestExchangeRatesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
