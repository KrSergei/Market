using Market.Abstractions;
using Market.Models;
using Market.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace Market.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private ProductContext _context;

        public CategoryController(IProductRepository productRepository, ProductContext productContext)
        {
            _productRepository = productRepository;
            _context = productContext;
        }

        [HttpGet(template: "get_categoryes")]
        public IActionResult GetCategoryes()
        {
            using (_context)
            {
                var categoryes = _productRepository.GetCategoryes();
                return Ok(categoryes);
            }
        }

        [HttpPost(template: "add_category")]
        public  IActionResult AddCategory([FromBody] CategoryDto categoryDto)
        {
            var result = _productRepository.AddCategory(categoryDto);
            return Ok(result);
        }

        [HttpDelete(template: "delete_catecory")]
        public IActionResult DeleteCategory([FromQuery] string name)
        {
            try
            {
                using (_context)
                {
                    var deletigRow = _context.Categoryes.FirstOrDefault(x => x.Name.ToLower().Equals(name));
                    if (deletigRow != null)
                    {
                        _context.Remove(deletigRow);
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
    }
}
