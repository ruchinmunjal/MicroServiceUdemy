using Microsoft.AspNetCore.Mvc;

namespace Order.Api.Controllers
{
    public class Order : Controller
    {
        // GET
        public IActionResult Index()
        {
            return View();
        }
    }
}