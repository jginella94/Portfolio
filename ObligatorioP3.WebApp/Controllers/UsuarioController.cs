using Microsoft.AspNetCore.Mvc;
using ObligatorioP3.LogicaAplicacion.DTOs.Usuarios;
using ObligatorioP3.LogicaNegocio.Excepciones;
using ObligatorioP3.LogicaNegocio.InterfacesLogicaAplicacion;
using ObligatorioP3.WebApp.Filtros;
using ObligatorioP3.WebApp.Models;

namespace ObligatorioP3.WebApp.Controllers
{
    [Admin]
    public class UsuarioController : Controller
    {
        private readonly IAlta<DtoUsuarioAlta> _alta;

        public UsuarioController(IAlta<DtoUsuarioAlta> alta)
        {
            _alta = alta;
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new VMUsuario());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(VMUsuario vmUsuario)
        {
            if (!ModelState.IsValid)
                return View(vmUsuario);

            try
            {
                DtoUsuarioAlta dto = new DtoUsuarioAlta
                {
                    NombreCompleto = vmUsuario.NombreCompleto,
                    NombreUsuario = vmUsuario.NombreUsuario,
                    Email = vmUsuario.Email,
                    Password = vmUsuario.Password,
                    Rol = vmUsuario.Rol
                };

                _alta.Ejecutar(dto);
                TempData["MensajeExito"] = "El usuario fue creado correctamente.";
                return RedirectToAction(nameof(Create));
            }
            catch (LogicaNegocioException e)
            {
                ModelState.AddModelError(string.Empty, e.Message);
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "No se pudo crear el usuario. Intente nuevamente.");
            }

            vmUsuario.Password = string.Empty;
            ModelState.Remove(nameof(VMUsuario.Password));
            return View(vmUsuario);
        }
    }
}
