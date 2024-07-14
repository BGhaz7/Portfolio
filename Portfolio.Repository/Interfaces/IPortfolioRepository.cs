using Portfolio.Models.Entities;

namespace Portfolio.Repository.Interfaces
{
    public interface IPortfolioRepository
    {
        Task<IEnumerable<UserPortfolio>> GetPortfolioByUserIdAsync(int userId);
        Task<UserPortfolio> GetProjectDetailsAsync(int userId, Guid projectId);
        Task CreatePortfolioAsync(UserPortfolio portfolio);
        Task AddInvestmentAsync(Investment investment);
    }
}