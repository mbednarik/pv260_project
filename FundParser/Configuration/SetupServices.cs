using DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Configuration
{
    public class SetupServices
    {
        public static void AddServices(IServiceCollection services, IConfigurationManager config)
        {
            services.AddDbContext<FundParserDbContext>(options =>
            {
                var folder = Environment.SpecialFolder.LocalApplicationData;
                var path = Environment.GetFolderPath(folder);
                var dbPath = Path.Join(path, config.GetConnectionString("DbName"));
                options.UseSqlite($"Data Source={dbPath}", b => b.MigrationsAssembly("FundParser"));
            });
        }
    }
}
