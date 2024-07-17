using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Portfolio.Models.Entities;
using Portfolio.Services.Interfaces;

namespace Portfolio.Controllers
{
    [ApiController]
    [Route("v1/[controller]")]
    public class PortfolioController : ControllerBase
    {
        private readonly IPortfolioService _portfolioService;
        
        public PortfolioController(IPortfolioService portfolioService)
        {
            _portfolioService = portfolioService;
        }
        
        [HttpGet("portfolio")]
        public async Task<ActionResult<IEnumerable<UserPortfolio>>> GetPortfolio()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            
            if (userIdClaim == null)
            {
                return Unauthorized("User ID not found in token.");
            }

            if (!int.TryParse(userIdClaim.Value, out var userId))
            {
                return BadRequest("Invalid user ID in token.");
            }

            var userPortfolio = await _portfolioService.GetPortfolioByUserIdAsync(userId);
            return Ok(userPortfolio);
        }
        
        [HttpGet("portfolio/{projectid}")]
        public async Task<ActionResult<IEnumerable<UserPortfolio>>> GetProjectDetails(Guid projectid)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            
            if (userIdClaim == null)
            {
                return Unauthorized("User ID not found in token.");
            }

            if (!int.TryParse(userIdClaim.Value, out var userId))
            {
                return BadRequest("Invalid user ID in token.");
            }

            var userPortfolioProject = await _portfolioService.GetProjectDetailsAsync(userId, projectid);
            return Ok(userPortfolioProject);
        }
    }
}
