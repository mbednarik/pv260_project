using AutoMapper;

using FundParser.BL.Services.CsvParsingService;
using FundParser.BL.Services.DownloaderService;
using FundParser.BL.Services.FundCsvService;
using FundParser.BL.Services.HoldingDiffCalculatorService;
using FundParser.BL.Services.HoldingDiffService;
using FundParser.BL.Services.HoldingService;
using FundParser.BL.Services.LoggingService;
using FundParser.DAL.Models;
using FundParser.DAL.Repository;
using FundParser.DAL.UnitOfWork;

using Microsoft.Extensions.DependencyInjection;

namespace FundParser.Configuration
{
    public static class StartupExtensions
    {
        public static void ConfigureDependencyInjection(this IServiceCollection services)
        {
            // Mapping DI Setup
            services.AddSingleton<IMapper>(new Mapper(new MapperConfiguration(MapperConfig.ConfigureMapper)));

            //Singleton DI Setup
            services.AddSingleton<IDownloaderService, DownloaderService>();
            services.AddSingleton<ILoggingService, LoggingService>();
            services.AddSingleton<ICsvParsingService<FundCsvRow>, CsvParsingService<FundCsvRow>>();
            services.AddSingleton<IHoldingDiffCalculatorService, HoldingDiffCalculatorService>();

            //Repository DI Setup
            services.AddScoped<IRepository<Holding>, Repository<Holding>>();
            services.AddScoped<IRepository<Fund>, Repository<Fund>>();
            services.AddScoped<IRepository<Company>, Repository<Company>>();
            services.AddScoped<IRepository<HoldingDiff>, Repository<HoldingDiff>>();

            // UnitOfWork DI Setup
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            //Services DI Setup
            services.AddScoped<IHoldingService, HoldingService>();
            services.AddScoped<IFundCsvService, FundCsvService>();
            services.AddScoped<IHoldingDiffService, HoldingDiffService>();
        }
    }
}
