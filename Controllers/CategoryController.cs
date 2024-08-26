using Market.Models;
using Microsoft.AspNetCore.Mvc;

namespace Market.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoryController : ControllerBase
    {
        [HttpPost(template: "postCatecory")]
        public  IActionResult PostCategory([FromQuery] string name, string description)
        {
            try
            {
                using (var context = new ProductContext())
                {
                    if (!context.Categoryes.Any(x => x.Name.ToLower().Equals(name)))
                    {
                        context.Add(new Category()
                        {
                            Name = name,
                            Description = description
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

        [HttpDelete(template: "deleteCatecory")]
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
