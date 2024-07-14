using Microsoft.EntityFrameworkCore;
using Portfolio.Models.Entities;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Portfolio.Repository.DbContext
{
    public class InvestmentContextFactory : IDesignTimeDbContextFactory<PortfolioContext>
    {
        public PortfolioContext CreateDbContext(string[] args)
        {
            var basePath = Directory.GetCurrentDirectory();
            var configuration = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile(Path.Combine(basePath, "../Portfolio.API/appsettings.json"))
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<PortfolioContext>();
            var connectionString = configuration.GetConnectionString("PostGresConnectionString");
            optionsBuilder.UseNpgsql(connectionString);

            return new PortfolioContext(optionsBuilder.Options);
        }
    }
}