using Market.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace Market.Controllers
{
    public class CashController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public CashController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpGet(template: "get_statistic_cash_url")]
        public ActionResult<string> GetProductSCVURL()
        {
            var content = _productRepository.GetCasheStatisticURL();
            string fileName = string.Empty;
            fileName = "Statistic-" + DateTime.Now.ToBinary().ToString() + ".scv";
            System.IO.File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory(), "StaticFiles", fileName), content);
            return "https://" + Request.Host.ToString() + "/statistic/" + fileName;
        }
    }
}
