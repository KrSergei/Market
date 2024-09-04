using Market.Models;
using Microsoft.AspNetCore.Mvc;

namespace Market.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductPriceController : ControllerBase
    {
        private ProductContext _context;

        public ProductPriceController(ProductContext productContext)
        {
            _context = productContext;
        }

        [HttpPatch(template: "patchPrice")]
        public IActionResult GetProducts([FromQuery] string productName, int price)
        {
            try
            {
                using (_context)
                {
                    var updatingProduct = _context.Products.FirstOrDefault(x => x.Name.ToLower().Equals(productName));
                    if (updatingProduct != null)
                    {
                        updatingProduct.Price = price;
                        _context.SaveChanges();                        
                    }
                    else
                    {
                        return NotFound();
                    }
                    return Ok();
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}
