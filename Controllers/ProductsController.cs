using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechnopurTask.Data;
using TechnopurTask.Models;

namespace TechnopurTask.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            return await _context.Products.ToListAsync();
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(Guid id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(Guid id, CreateOrUpdateProduct product)
        {
            if (product is null)
            {
                return NotFound("Product not found!");
            }

            var updatedProduct = _context.Products.Where(p => p.Id == id).FirstOrDefault();

            updatedProduct.Price = product.Price;
            updatedProduct.Name = product.Name;

            _context.Products.Update(updatedProduct);
            await _context.SaveChangesAsync();

            return Ok("Product updated successfully.");
        }

        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(CreateOrUpdateProduct product)
        {
            var newProduct = new Product
            {
                Name = product.Name,
                Price = product.Price
            };

            _context.Products.Add(newProduct);
            await _context.SaveChangesAsync();

            return Ok("Product Saved successfully.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return Ok("Product deleted successfully.");
        }

        private bool ProductExists(Guid id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
