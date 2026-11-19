using Microsoft.AspNetCore.Mvc;
using Dominio;

namespace MVC.Controllers
{
    public class TiposGastosController : Controller
    {
        Sistema miSistema = Sistema.Instancia;

        [HttpGet]
        public IActionResult EliminarTipoGasto()
        {
            if (HttpContext.Session.GetString("rol") != "GERENTE") return View("NoAuth");
            return View();
        }

        [HttpPost]
        public IActionResult EliminarTipoGasto(string nomTipoGasto)
        {
            if (HttpContext.Session.GetString("rol") != "GERENTE") return View("NoAuth");
            try
            {
                if (string.IsNullOrEmpty(nomTipoGasto)) throw new Exception("El tipo de gasto no puede ser nulo");
                TipoGasto tg = miSistema.BuscarTipoGasto(nomTipoGasto);
                if (tg == null) throw new Exception("No se encotro el tipo de gasto.");
                miSistema.EliminarTipoGasto(tg);
                ViewBag.Exito = "Se ha eliminado el Tipo de Gasto de manera correcta";
            }
            catch(Exception ex)
            {
                ViewBag.Error = ex.Message;
            }
            return View();
        }
    }
}
