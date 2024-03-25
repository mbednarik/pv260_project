using AutoMapper;

using FundParser.BL.Services.FundCsvService;
using FundParser.BL.Services.HoldingDiffService;
using FundParser.BL.Services.HoldingService;
using FundParser.DAL.Csv;
using FundParser.DAL.Models;
using FundParser.DAL.Repository;
using FundParser.DAL.UnitOfWork;

using Microsoft.Extensions.DependencyInjection;

namespace FundParser.Configuration
{
    public class ServiceConfig
    {
        public static void ConfigureDependencyInjection(IServiceCollection services)
        {
            // Mapping DI Setup
            services.AddSingleton<IMapper>(new Mapper(new MapperConfiguration(MapperConfig.ConfigureMapper)));

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

            services.AddScoped<ICsvDownloader<FundCsvRow>, CsvDownloader<FundCsvRow>>();
            services.AddScoped<IHoldingDiffService, HoldingDiffService>();
        }
    }
}
