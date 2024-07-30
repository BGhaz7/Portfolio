using Portfolio.Models.Entities;
using Portfolio.Repository.Interfaces;
using Portfolio.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using Portfolio.Services.Interfaces;


namespace Portfolio.Services.Services
{
    public class PortfolioService : IPortfolioService
    {
        private readonly IPortfolioRepository _portfolioRepository;

        public PortfolioService(IPortfolioRepository portfolioRepository)
        {
            _portfolioRepository = portfolioRepository;
        }

        public async Task<IEnumerable<UserPortfolio>> GetPortfolioByUserIdAsync(int userId)
        {
            return await _portfolioRepository.GetPortfolioByUserIdAsync(userId);
        }

        public async Task<UserPortfolio> GetProjectDetailsAsync(int userId, Guid projectId)
        {
            return await _portfolioRepository.GetProjectDetailsAsync(userId, projectId);
        }

        public async Task CreatePortfolioAsync(int userId)
        {
            var portfolio = new UserPortfolio { UserId = userId };
            await _portfolioRepository.CreatePortfolioAsync(portfolio);
        }

        public async Task AddInvestmentAsync(int userId, Guid projectId, decimal amount, UserPortfolio userPortfolio)
        {
            var investment = new Investment
            {
                UserId = userId,
                ProjectId = projectId,
                Amount = amount
            };
            Console.WriteLine("Hello from Service");
            Console.WriteLine($"Investment: {investment}, UserPortfolio: {userPortfolio}");

            try
            {
                await _portfolioRepository.AddInvestmentAsync(investment, userPortfolio);
                Console.WriteLine("Investment added successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in service method: {ex.Message}");
            }
        }
    }
}