using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi_with_QuestPDF.Models;

namespace WebApi_with_QuestPDF.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        public ProductController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        [HttpGet("GetAllProducts")]
        public async Task<IActionResult> GetAllProducts()
        {
            return Ok(await _dbContext.Products.ToListAsync());
        }
        [HttpPost("AddProduct")]
        public async Task<IActionResult> AddProduct(Product product)
        {
           await _dbContext.Products.AddAsync(product);
           await _dbContext.SaveChangesAsync();

            return Ok(product);
        }
       
    }
}
