using InvestmentOrders.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InvestmentOrders.Api.Controllers
{
    [ApiController]
    [Route("api/order-status")]
    public class OrderStatusController : ControllerBase
    {
        private readonly InvestmentDbContext _context;

        public OrderStatusController(InvestmentDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var statuses = await _context.OrderStatuses
                .Select(s => new
                {
                    s.Id,
                    s.DescripcionEstado
                })
                .ToListAsync();

            return Ok(statuses);
        }
    }

}
