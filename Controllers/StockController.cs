using api.Data;
using api.DTOs.Stock;
using api.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/stocks")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public StockController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("")]
        public IActionResult GetAll()
        {
            var stocks = _context.Stocks.ToList().Select(s => s.ToStockDto());
            return Ok(stocks);
        }

        [HttpGet("{id}")]
        public IActionResult GetById([FromRoute] int id)
        {
            var stock = _context.Stocks.Find(id);
            if (stock == null)
            {
                return NotFound();
            }
            return Ok(stock.ToStockDto());
        }

        [HttpPost("")]
        public IActionResult Create([FromBody] CreateStockRequestDto stockDto)
        {
            var stock = stockDto.ToStockFromCreateDto();
            _context.Stocks.Add(stock);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetById), new { id = stock.Id }, stock.ToStockDto());
        }

        [HttpPut("{id}")]
        public IActionResult Update([FromRoute] int id, [FromBody] UpdateStockRequest updateStock)
        {
            var stock = _context.Stocks.FirstOrDefault(s => s.Id == id);
            if (stock == null)
            {
                return NotFound();
            }
            stock.Purchase = updateStock.Purchase;
            stock.Symbol = updateStock.Symbol;
            stock.MarketCap = updateStock.MarketCap;
            stock.CompanyName = updateStock.CompanyName;
            stock.LastDiv = updateStock.LastDiv;
            stock.Industry = updateStock.Industry;
            _context.SaveChanges();
            return Ok(stock.ToStockDto());
        }

        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute] int id)
        {
            var stock = _context.Stocks.Find(id);
            if (stock == null)
            {
                return NotFound();
            }
            _context.Stocks.Remove(stock);
            _context.SaveChanges();
            return NoContent();
        }
    }
}