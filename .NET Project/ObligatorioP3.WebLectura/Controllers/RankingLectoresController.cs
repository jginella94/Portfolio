using Microsoft.AspNetCore.Mvc;
using ObligatorioP3.WebLectura.Models;
using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;

namespace ObligatorioP3.WebLectura.Controllers
{
    public class RankingLectoresController : Controller
    {
        private readonly IHttpClientFactory _factory;
        private readonly JsonSerializerOptions _options;

        public RankingLectoresController(IHttpClientFactory factory, JsonSerializerOptions options)
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

            VMRankingLectores vm = new();

            try
            {
                HttpClient client = _factory.CreateClient("Api");
                using HttpRequestMessage solicitud = new(HttpMethod.Get, "api/RankingLectores");
                solicitud.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                using HttpResponseMessage respuesta = client.Send(solicitud);

                if (respuesta.StatusCode == HttpStatusCode.Unauthorized)
                {
                    HttpContext.Session.Clear();
                    return RedirectToAction("Index", "Login");
                }

                if (respuesta.StatusCode == HttpStatusCode.Forbidden)
                {
                    vm.MensajeError = "Solo los administradores pueden consultar el ranking de lectores.";
                    return View(vm);
                }

                if (!respuesta.IsSuccessStatusCode)
                {
                    vm.MensajeError = "No se pudo consultar el ranking de lectores.";
                    return View(vm);
                }

                string json = respuesta.Content.ReadAsStringAsync().Result;
                vm.Lectores = JsonSerializer.Deserialize<List<DtoRankingLectorApi>>(json, _options) ?? new();
                return View(vm);
            }
            catch (HttpRequestException)
            {
                vm.MensajeError = "No se pudo conectar con la API.";
                return View(vm);
            }
            catch (JsonException)
            {
                vm.MensajeError = "La API devolvió datos con un formato inválido.";
                return View(vm);
            }
        }
    }
}
