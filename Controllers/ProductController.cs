using Market.Abstractions;
using Market.Models;
using Market.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace Market.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductController : ControllerBase
{  
    private readonly IProductRepository _productRepository;
    private ProductContext _context;

    public ProductController(IProductRepository productRepository, ProductContext context)
    {
        _productRepository = productRepository;
        _context = context;
    }

    [HttpGet(template: "get_products")]
    public IActionResult GetProducts()
    {
        using (_context)
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
            using (_context)
            {
                var deletingProduct = _context.Products.FirstOrDefault(x => x.Name.ToLower().Equals(name));
                if (deletingProduct != null)
                {
                    _context.Remove(deletingProduct);
                    _context.SaveChanges();
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

    [HttpGet(template: "get_products_csv")]
    public FileContentResult GetProductsCSV()
    {
        using (_context)
        {
            var products = _productRepository.GetProducts();
            var content = GetSCV(products);
            return File(new UTF8Encoding().GetBytes(content), "text/scv", "report.scv");
        }
    }
    private string GetSCV(IEnumerable<ProductDto> products)
    {
        StringBuilder sb = new StringBuilder();
        foreach (var product in products)
        {
            sb.Append($"{product.Name} : {product.Description} : {product.Price}");
        }
        return sb.ToString();
    }
}
