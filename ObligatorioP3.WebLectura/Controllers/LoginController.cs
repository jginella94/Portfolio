using Microsoft.AspNetCore.Mvc;
using ObligatorioP3.WebLectura.Models;
using System.Text;
using System.Text.Json;

namespace ObligatorioP3.WebLectura.Controllers
{
    public class LoginController : Controller
    {
        private readonly IHttpClientFactory _factory;
        private readonly JsonSerializerOptions _options;

        public LoginController(IHttpClientFactory factory, JsonSerializerOptions options)
        {
            _factory = factory;
            _options = options;
        }

        [HttpGet]
        public IActionResult Index()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("token")))
                return RedirectToAction("Index", "Perfil");

            return View(new VMLogin());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(VMLogin login)
        {
            if (!ModelState.IsValid)
                return View("Index", login);

            try
            {
                HttpClient client = _factory.CreateClient("Api");
                using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "api/Login");
                request.Content = new StringContent(JsonSerializer.Serialize(login, _options), Encoding.UTF8, "application/json");
                using HttpResponseMessage response = client.Send(request);

                if (!response.IsSuccessStatusCode)
                {
                    ModelState.AddModelError(string.Empty, response.StatusCode == System.Net.HttpStatusCode.Unauthorized
                        ? "Usuario o contraseña incorrectos."
                        : "No fue posible iniciar sesión.");
                    return View("Index", login);
                }

                string json = response.Content.ReadAsStringAsync().Result;
                DtoTokenApi? jwt = JsonSerializer.Deserialize<DtoTokenApi>(json, _options);

                if (string.IsNullOrWhiteSpace(jwt?.Token))
                {
                    ModelState.AddModelError(string.Empty, "La API no devolvió un token válido.");
                    return View("Index", login);
                }

                HttpContext.Session.SetString("token", jwt.Token);
                return RedirectToAction("Index", "Perfil");
            }
            catch (HttpRequestException)
            {
                ModelState.AddModelError(string.Empty, "No se pudo conectar con la API.");
                return View("Index", login);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction(nameof(Index));
        }
    }
}
