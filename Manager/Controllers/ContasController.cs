using Microsoft.AspNetCore.Mvc;

namespace Manager.Controllers
{
    public class ContasController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
