using Market.Models;
using Microsoft.AspNetCore.Mvc;

namespace Market.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductController : ControllerBase
{
    //1.51.55
    
    [HttpGet(template: "getProduct")]
    public IActionResult GetProducts()
    {
		try
		{
			using (var context = new ProductContext())
			{
				var products = context.Products.Select(x => new Product()
				{ 
					Id = x.Id,
					Name = x.Name,
					Description = x.Description
				});
				return Ok(products);
			}
		}
		catch (Exception)
		{
			return StatusCode(500);
		}
    }

    [HttpPost(template: "postProduct")]
    public IActionResult PostProducts([FromQuery] string name, string description, int categoryId, int price)
    {
        try
        {
            using (var context = new ProductContext())
            {
                if(!context.Products.Any(x => x.Name.ToLower().Equals(name)))
                {
                    context.Add(new Product()
                    {
                        Name = name,
                        Description = description,
                        Category_Id = categoryId,
                        Price = price
                    });
                    context.SaveChanges();
                    return Ok();
                }
                else
                {
                    return StatusCode(409);
                }
            }
        }
        catch (Exception)
        {
            return StatusCode(500);
        }
    }    
    
    [HttpDelete(template: "deleteProduct")]
    public IActionResult delteProducts([FromQuery] string name)
    {
        try
        {
            using (var context = new ProductContext())
            {
                var deletingProduct = context.Products.FirstOrDefault(x => x.Name.ToLower().Equals(name));
                if (deletingProduct != null)
                {
                    context.Remove(deletingProduct);    
                    context.SaveChanges();
                    return Ok();
                }
                else
                {
                    return StatusCode(409);
                }
            }
        }
        catch (Exception)
        {
            return StatusCode(500);
        }
    }
}
