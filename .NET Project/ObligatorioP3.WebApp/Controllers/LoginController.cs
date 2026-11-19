using Microsoft.AspNetCore.Mvc;
using ObligatorioP3.LogicaNegocio.InterfacesLogicaAplicacion;
using ObligatorioP3.WebApp.Models;

namespace ObligatorioP3.WebApp.Controllers
{
    public class LoginController : Controller
    {
        private readonly IAutenticar _autenticar;

        public LoginController(IAutenticar autenticar)
        {
            _autenticar = autenticar;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(VMLogin login)
        {
            if (!ModelState.IsValid)
                return View("Index", login);

            int? idUsuario = _autenticar.Ejecutar(login.Mail, login.Password);

            if (idUsuario is null)
            {
                ViewBag.ErrorMessage = "Nombre de usuario o contraseña incorrectos.";
                login.Password = string.Empty;
                ModelState.Remove(nameof(VMLogin.Password));
                return View("Index", login);
            }

            HttpContext.Session.Clear();
            HttpContext.Session.SetInt32("UsuarioId", idUsuario.Value);
            HttpContext.Session.SetString("User", login.Mail.Trim());
            HttpContext.Session.SetString("Rol", "admin");

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
    }
}
