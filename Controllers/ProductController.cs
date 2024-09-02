using Market.Abstractions;
using Market.Models;
using Market.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace Market.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductController : ControllerBase
{  
    private readonly IProductRepository _productRepository;

    public ProductController(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    [HttpGet(template: "get_products")]
    public IActionResult GetProducts()
    {
        using (var context = new ProductContext())
        {
            var products = _productRepository.GetProducts();
            return Ok(products);
        }
    }

    [HttpPost(template: "add_products")]
    public IActionResult AddProduct([FromBody] ProductDto productDto)
    {
        var result = _productRepository.AddProduct(productDto);
        return Ok(result);
    }    
    
    [HttpDelete(template: "delete_products")]
    public IActionResult DeleteProducts([FromQuery] string name)
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
