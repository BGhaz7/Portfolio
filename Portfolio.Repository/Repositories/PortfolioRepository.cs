
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
            return await _context.Portfolios
                .Include(p => p.Investments)
                .Where(p => p.UserId == userId)
                .ToListAsync();
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

        public async Task AddInvestmentAsync(Investment investment)
        {
            await _context.Investments.AddAsync(investment);
            await _context.SaveChangesAsync();
        }
    }
}