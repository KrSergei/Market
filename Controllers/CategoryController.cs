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

        public CategoryController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpGet(template: "get_categoryes")]
        public IActionResult GetCategoryes()
        {
            using (var context = new ProductContext())
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
                using (var context = new ProductContext())
                {
                    var deletigRow = context.Categoryes.FirstOrDefault(x => x.Name.ToLower().Equals(name));
                    if (deletigRow != null)
                    {
                        context.Remove(deletigRow);
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
}
