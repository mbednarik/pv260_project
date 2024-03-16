using AutoMapper;
using BL.Services.HoldingDiffService;
using BL.Services.HoldingService;
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
            services.AddScoped<IRepository<HoldingDiff>, Repository<HoldingDiff>>();

            // UnitOfWork DI Setup
            services.AddScoped<IUoWHolding, UoWHolding>();
            services.AddScoped<IUoWHoldingDiff, UoWHoldingDiff>();

            //Services DI Setup
            services.AddScoped<IHoldingService, HoldingService>();
            services.AddScoped<IHoldingDiffService, HoldingDiffService>();
        }
    }
}
