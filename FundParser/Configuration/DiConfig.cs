using AutoMapper;
using BL.Services.CompanyService;
using BL.Services.CsvService;
using BL.Services.FundService;
using BL.Services.HoldingService;
using DAL.Csv;
using DAL.Models;
using DAL.Repository;
using DAL.UnitOfWork;
using DAL.UnitOfWork.Interface;
using Microsoft.Extensions.DependencyInjection;

namespace Configuration
{
    public class DiConfig
    {
        public static void ConfigureDi(IServiceCollection services)
        {
            // Mapping DI Setup
            services.AddSingleton<IMapper>(new Mapper(new MapperConfiguration(MappingConfig.ConfigureMapping)));

            //Repository DI Setup
            services.AddScoped<IRepository<Holding>, Repository<Holding>>();
            services.AddScoped<IRepository<Fund>, Repository<Fund>>();
            services.AddScoped<IRepository<Company>, Repository<Company>>();

            // UnitOfWork DI Setup
            services.AddScoped<IUoWHolding, UoWHolding>();
            services.AddScoped<IUoWFund, UoWFund>();
            services.AddScoped<IUoWCompany, UoWCompany>();

            //Services DI Setup
            services.AddScoped<IHoldingService, HoldingService>();
            services.AddScoped<IFundService, FundService>();
            services.AddScoped<ICompanyService, CompanyService>();
            services.AddScoped<ICsvService, CsvService>();

            services.AddScoped<CsvDownloader<FundCsvRow>>();
        }
    }
}
