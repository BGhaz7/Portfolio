using Microsoft.EntityFrameworkCore;
using Portfolio.Models.Entities;

namespace Portfolio.Repository.DbContext
{
    public class PortfolioContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public PortfolioContext(DbContextOptions<PortfolioContext> options) : base(options)
        {
        }

        public DbSet<UserPortfolio> Portfolios { get; set; }
        public DbSet<Investment> Investments { get; set; }
    }
}