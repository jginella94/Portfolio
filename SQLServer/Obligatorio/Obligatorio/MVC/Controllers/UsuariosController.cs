using Dominio;
using Microsoft.AspNetCore.Mvc;

namespace MVC.Controllers
{
    public class UsuariosController : Controller
    {
        Sistema miSistema = Sistema.Instancia;

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string pass)
        {
            try
            {
                if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(pass)) throw new Exception("Los campos no pueden estar vacios");
                Usuario u = miSistema.Login(email.ToLower(), pass);
                if (u == null) throw new Exception("Email o contraseña incorrectos");

                HttpContext.Session.SetString("email", u.Email);
                HttpContext.Session.SetString("rol", u.Rol.ToString());
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View();
            }
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return View("Login");
        }

        public IActionResult Perfil()
        {
            if (HttpContext.Session.GetString("email") == null) return View("NoAuth");
            Usuario u = miSistema.BuscarUsuario(HttpContext.Session.GetString("email"));
            ViewBag.Usuario = u;
            ViewBag.Gastos = miSistema.CalcularGastoTotalDelMesActual(u);
            if (HttpContext.Session.GetString("rol") == "GERENTE")
            {
                List<Usuario> usuarios = miSistema.ListarUsuariosPorEquipo(u.Equipo);
                usuarios.Sort();
                ViewBag.Listado = usuarios;
            }
            return View();
        }



    }
}
