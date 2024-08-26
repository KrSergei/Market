using Market.Models;
using Microsoft.AspNetCore.Mvc;

namespace Market.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductPriceController : ControllerBase
    {
        [HttpPatch(template: "patchPrice")]
        public IActionResult GetProducts([FromQuery] string productName, int price)
        {
            try
            {
                using (var context = new ProductContext())
                {
                    var updatingProduct = context.Products.FirstOrDefault(x => x.Name.ToLower().Equals(productName));
                    if (updatingProduct != null)
                    {
                        updatingProduct.Price = price;
                        context.SaveChanges();                        
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
