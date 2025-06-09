using Microsoft.AspNetCore.Mvc;

namespace OnlinePaymentSite.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            ViewData["FullName"] = HttpContext.Session.GetString("FullName");
            return View();
        }
    }
}
