using InvestmentOrders.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InvestmentOrders.Api.Controllers
{
    [ApiController]
    [Route("api/asset-types")]
    public class AssetTypesController : ControllerBase
    {
        private readonly InvestmentDbContext _context;

        public AssetTypesController(InvestmentDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var types = await _context.AssetTypes
                .Select(t => new
                {
                    t.Id,
                    t.Description
                })
                .ToListAsync();

            return Ok(types);
        }
    }

}
