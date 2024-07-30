
using Microsoft.EntityFrameworkCore;
using Portfolio.Models.Entities;
using Portfolio.Repository.DbContext;
using Portfolio.Repository.Interfaces;

namespace Portfolio.Repository.Repositories
{
    public class PortfolioRepository : IPortfolioRepository
    {
        private readonly PortfolioContext _context;

        public PortfolioRepository(PortfolioContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UserPortfolio>> GetPortfolioByUserIdAsync(int userId)
        {
            var portfolios =  await _context.Portfolios
                .Include(p => p.Investments)
                .Where(p => p.UserId == userId)
                .ToListAsync();
            Console.WriteLine($"Retrieved {portfolios.Count} portfolios for user ID {userId}");
            return portfolios;
        }

        public async Task<UserPortfolio> GetProjectDetailsAsync(int userId, Guid projectId)
        {
            return await _context.Portfolios
                .Include(p => p.Investments)
                .FirstOrDefaultAsync(p => p.UserId == userId && p.Investments.Any(i => i.ProjectId == projectId));
        }

        public async Task CreatePortfolioAsync(UserPortfolio portfolio)
        {
            await _context.Portfolios.AddAsync(portfolio);
            await _context.SaveChangesAsync();
        }

        public async Task AddInvestmentAsync(Investment investment, UserPortfolio userPortfolio)
        {
            investment.UserPortfolio = userPortfolio;
            Console.WriteLine($"Adding investment to user portfolio: {userPortfolio.Id}");
    
            try
            {
                await _context.Investments.AddAsync(investment);
                await _context.SaveChangesAsync();
                Console.WriteLine("Investment saved to database successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in repository method: {ex.Message}");
            }
        }
    }
}