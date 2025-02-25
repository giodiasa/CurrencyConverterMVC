using CurrencyConverter.Application;
using CurrencyConverter.Application.Interfaces;
using CurrencyConverter.Application.Services;
using CurrencyConverter.Core.Interfaces;
using CurrencyConverter.Infrastructure;
using CurrencyConverter.Infrastructure.Repositories;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.EntityFrameworkCore;

namespace CurrencyConverter
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var services = builder.Services;

            services.AddHangfire(config => config
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseSqlServerStorage(builder.Configuration.GetConnectionString("CurrencyConverterDb")));
            services.AddHangfireServer();

            services.AddControllersWithViews();

            services.AddDbContext<CurrencyConverterDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("CurrencyConverterDb")));

            services.AddScoped<IExchangeRateRepository, ExchangeRateRepository>();
            services.AddScoped<ITransactionRepository, TransactionRepository>();
            services.AddScoped<ICurrencyConversionService, CurrencyConversionService>();
            services.AddScoped<ITransactionService, TransactionService>();

            services.AddAutoMapper(typeof(MappingProfile));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseHangfireDashboard();

            var recurringJobManager = app.Services.GetRequiredService<IRecurringJobManager>();
            var backgroundJobClient = app.Services.GetRequiredService<IBackgroundJobClient>();

            recurringJobManager.AddOrUpdate<CurrencyConversionService>(
                "FetchAndStoreLatestExchangeRates",
                service => service.FetchAndStoreLatestExchangeRatesAsync(),
                Cron.Daily(2, 0)
            );
            backgroundJobClient.Enqueue<CurrencyConversionService>(
                service => service.FetchAndStoreLatestExchangeRatesAsync()
);

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=CurrencyConversion}/{action=Index}/{id?}");

            

            app.Run();
        }
    }
}
