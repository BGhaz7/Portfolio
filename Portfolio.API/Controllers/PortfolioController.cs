using System.IdentityModel.Tokens.Jwt;
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
            // Extract token from the Authorization header
            var authHeader = Request.Headers["Authorization"].ToString();
            if (string.IsNullOrEmpty(authHeader))
            {
                return Unauthorized("Authorization header is missing.");
            }

            var token = authHeader.Substring("Bearer ".Length).Trim();

            // Read the token and extract claims
            var handler = new JwtSecurityTokenHandler();
            if (handler.ReadToken(token) is JwtSecurityToken jsonToken)
            {
                var nameIdClaim = jsonToken.Claims.FirstOrDefault(claim => claim.Type == JwtRegisteredClaimNames.NameId);

                if (nameIdClaim == null)
                {
                    return Unauthorized("User ID not found in token.");
                }

                if (!int.TryParse(nameIdClaim.Value, out var userId))
                {
                    return BadRequest("Invalid user ID in token.");
                }

                var userPortfolio = await _portfolioService.GetPortfolioByUserIdAsync(userId);
                return Ok(userPortfolio);
            }

            return BadRequest("Invalid token.");
        }


        
        [HttpGet("portfolio/{projectid}")]
        public async Task<ActionResult<IEnumerable<UserPortfolio>>> GetProjectDetails(Guid projectid)
        {
            var authHeader = Request.Headers["Authorization"].ToString();
            if (string.IsNullOrEmpty(authHeader))
            {
                return Unauthorized("Authorization header is missing.");
            }

            var token = authHeader.Substring("Bearer ".Length).Trim();

            // Read the token and extract claims
            var handler = new JwtSecurityTokenHandler();
            if (handler.ReadToken(token) is JwtSecurityToken jsonToken)
            {
                var nameIdClaim =
                    jsonToken.Claims.FirstOrDefault(claim => claim.Type == JwtRegisteredClaimNames.NameId);

                if (nameIdClaim == null)
                {
                    return Unauthorized("User ID not found in token.");
                }

                if (!int.TryParse(nameIdClaim.Value, out var userId))
                {
                    return BadRequest("Invalid user ID in token.");
                }
                var userPortfolioProject = await _portfolioService.GetProjectDetailsAsync(userId, projectid);
                return Ok(userPortfolioProject);
            }

            return BadRequest("Invalid Token");
        }
    }
}
