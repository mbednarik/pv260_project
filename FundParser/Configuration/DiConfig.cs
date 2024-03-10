using AutoMapper;
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

            // UnitOfWork DI Setup
            services.AddScoped<IUoWHolding, UoWHolding>();

            //Services DI Setup
            services.AddScoped<IHoldingService, HoldingService>();
        }
    }
}
