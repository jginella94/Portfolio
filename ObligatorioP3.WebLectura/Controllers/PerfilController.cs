using Microsoft.AspNetCore.Mvc;
using ObligatorioP3.WebLectura.Models;
using System.Net.Http.Headers;
using System.Text.Json;

namespace ObligatorioP3.WebLectura.Controllers
{
    public class PerfilController : Controller
    {
        private readonly IHttpClientFactory _factory;
        private readonly JsonSerializerOptions _options;

        public PerfilController(IHttpClientFactory factory, JsonSerializerOptions options)
        {
            _factory = factory;
            _options = options;
        }

        [HttpGet]
        public IActionResult Index()
        {
            string? token = HttpContext.Session.GetString("token");
            if (string.IsNullOrWhiteSpace(token))
                return RedirectToAction("Index", "Login");

            try
            {
                HttpClient client = _factory.CreateClient("Api");
                using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "api/Perfil");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                using HttpResponseMessage response = client.Send(request);

                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    HttpContext.Session.Clear();
                    return RedirectToAction("Index", "Login");
                }

                if (!response.IsSuccessStatusCode)
                {
                    TempData["MensajeError"] = "No se pudo obtener el perfil.";
                    return RedirectToAction("Index", "Login");
                }

                string json = response.Content.ReadAsStringAsync().Result;
                DtoPerfilApi? perfil = JsonSerializer.Deserialize<DtoPerfilApi>(json, _options);
                if (perfil is null)
                {
                    TempData["MensajeError"] = "La API devolvió un perfil inválido.";
                    return RedirectToAction("Index", "Login");
                }

                if (!string.IsNullOrWhiteSpace(perfil.Rol))
                    HttpContext.Session.SetString("rol", perfil.Rol);

                return View(perfil);
            }
            catch (HttpRequestException)
            {
                TempData["MensajeError"] = "No se pudo conectar con la API.";
                return RedirectToAction("Index", "Login");
            }
        }
    }
}
