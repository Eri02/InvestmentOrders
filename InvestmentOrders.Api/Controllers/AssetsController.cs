using InvestmentOrders.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InvestmentOrders.Api.Controllers
{
    [ApiController]
    [Route("api/assets")]
    public class AssetsController : ControllerBase
    {
        private readonly InvestmentDbContext _context;

        public AssetsController(InvestmentDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var assets = await _context.Assets
                .Include(a => a.AssetType)
                .Select(a => new
                {
                    a.Id,
                    a.Ticker,
                    a.Name,
                    a.UnitPrice,
                    AssetType = a.AssetType.Description
                })
                .ToListAsync();

            return Ok(assets);
        }
    }

}
