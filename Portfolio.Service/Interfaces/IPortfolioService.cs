using Portfolio.Models.Entities;

namespace Portfolio.Services.Interfaces
{
    public interface IPortfolioService
    {
        public Task<IEnumerable<UserPortfolio>> GetPortfolioByUserIdAsync(int userId);
        public Task<UserPortfolio> GetProjectDetailsAsync(int userId, Guid projectId);
        public Task CreatePortfolioAsync(int userId);
        public Task AddInvestmentAsync(int userId, Guid projectId, decimal amount, UserPortfolio userPortfolio);
    }
}