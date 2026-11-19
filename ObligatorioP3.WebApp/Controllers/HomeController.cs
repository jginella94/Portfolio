using Microsoft.AspNetCore.Mvc;
using ObligatorioP3.WebApp.Models;
using System.Diagnostics;
using ObligatorioP3.WebApp.Filtros;

namespace ObligatorioP3.WebApp.Controllers
{
    public class HomeController : Controller
    {
        [Admin]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
