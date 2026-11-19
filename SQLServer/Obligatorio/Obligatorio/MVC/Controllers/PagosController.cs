using Dominio;
using Microsoft.AspNetCore.Mvc;

namespace MVC.Controllers
{
    public class PagosController : Controller
    {
        Sistema miSistema = Sistema.Instancia;
        public IActionResult ListarMisPagos()
        {
            if (HttpContext.Session.GetString("email") == null) return View("NoAuth");
            try
            {
                ViewBag.Listado = miSistema.BuscarPagosPorEmailMesActual(HttpContext.Session.GetString("email"));
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View();
            }
        }

        [HttpGet]
        public IActionResult CargarPagoUnico()
        {
            if (HttpContext.Session.GetString("email") == null) return View("NoAuth");
            return View();
        }

        [HttpPost]
        public IActionResult CargarPagoUnico(MetodoPago metodoPago, string tipoGasto, string descripcion, double monto, DateTime fecha, int nrorecibo)
        {
            if (HttpContext.Session.GetString("email") == null) return View("NoAuth");
            try
            {
                if (string.IsNullOrEmpty(descripcion)) throw new Exception("La descripcion no puede ser vacia");
                if (monto <= 0) throw new Exception("El monto debe ser positivo");
                if (nrorecibo <= 0) throw new Exception("El numero de recibo debe ser mayor a 0");
                if (fecha.CompareTo(DateTime.Now) == 1) throw new Exception("La fecha no puede ser en el futuro");
                Usuario u = miSistema.BuscarUsuario(HttpContext.Session.GetString("email"));
                TipoGasto t = miSistema.BuscarTipoGasto(tipoGasto);
                if (t == null) throw new Exception("No se encotro el tipo de gasto.");
                Unico p = new Unico(metodoPago, t, u, descripcion, monto, fecha, nrorecibo);
                miSistema.CrearPagoUnico(p);
                ViewBag.Exito = "Pago creado";
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }
            return View();
        }

        [HttpGet]
        public IActionResult CargarPagoRecurrente()
        {
            if (HttpContext.Session.GetString("email") == null) return View("NoAuth");
            return View();
        }

        [HttpPost]
        public IActionResult CargarPagoRecurrente(MetodoPago metodoPago, string tipoGasto, string descripcion, double monto, DateTime fechai, DateTime? fechaf)
        {
            if (HttpContext.Session.GetString("email") == null) return View("NoAuth");
            try
            {
                if (string.IsNullOrEmpty(descripcion)) throw new Exception("La descripcion no puede ser vacia");
                if (monto <= 0) throw new Exception("El monto debe ser positivo");
                if (fechai.CompareTo(DateTime.Now) == 1) throw new Exception("La fecha de inicio no puede ser en el futuro");
                if (fechaf != null && fechaf < fechai) throw new Exception("la fecha de fin debe ser mayor a la de inicio");
                Usuario u = miSistema.BuscarUsuario(HttpContext.Session.GetString("email"));
                TipoGasto t = miSistema.BuscarTipoGasto(tipoGasto);
                if (t == null) throw new Exception("No se encotro el tipo de gasto.");
                Recurrente p = new Recurrente(metodoPago, t, u, descripcion, monto, fechai, fechaf);
                miSistema.CrearPagoRecurrente(p);
                ViewBag.Exito = "Pago creado";
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }
            return View();
        }

        public IActionResult VerGastosEquipo(int? m, int? a)
        {
            if (HttpContext.Session.GetString("rol") != "GERENTE") return View("NoAuth");
            ViewBag.Listado = new List<Pago>();
            try
            {
                Usuario u = miSistema.BuscarUsuario(HttpContext.Session.GetString("email"));
                List<Usuario> usuarios = miSistema.ListarUsuariosPorEquipo(u.Equipo);
                int mes = DateTime.Now.Month;
                int anio = DateTime.Now.Year;
                if (m != null || a != null)
                {
                    if (m == null || m < 1 && m > 12) throw new Exception("El mes debe estar entre 1 y 12");
                    if (a == null || a < 1900 && a > anio) throw new Exception("El año debe ser mayor a 1900 y no puede ser en el futuro");

                    mes = (int)m;
                    anio = (int)a;
                }

                ViewBag.Listado = miSistema.ListarPagosPorEquipo(usuarios, mes, anio);
            }
            catch(Exception ex)
            {
                ViewBag.Error = ex.Message;
            }

            return View();

        }
    }
}



