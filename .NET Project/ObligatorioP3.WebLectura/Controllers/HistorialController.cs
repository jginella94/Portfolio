using Microsoft.AspNetCore.Mvc;
using ObligatorioP3.WebLectura.Models;
using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;

namespace ObligatorioP3.WebLectura.Controllers
{
    public class HistorialController : Controller
    {
        private readonly IHttpClientFactory _factory;
        private readonly JsonSerializerOptions _options;

        public HistorialController(IHttpClientFactory factory, JsonSerializerOptions options)
        {
            _factory = factory;
            _options = options;
        }

        [HttpGet]
        public IActionResult Index(int? mes, int? anio)
        {
            string? token = HttpContext.Session.GetString("token");
            if (string.IsNullOrWhiteSpace(token))
                return RedirectToAction("Index", "Login");

            DateTime hoy = DateTime.UtcNow;
            VMHistorialLecturas vm = new()
            {
                Mes = mes ?? hoy.Month,
                Anio = anio ?? hoy.Year
            };

            if (vm.Mes < 1 || vm.Mes > 12 || vm.Anio < 1 || vm.Anio > 9998)
            {
                vm.MensajeError = "El mes o el año no son válidos.";
                return View(vm);
            }

            try
            {
                HttpClient client = _factory.CreateClient("Api");
                using HttpRequestMessage solicitud = new(HttpMethod.Get,
                    $"api/Lecturas/historial?mes={vm.Mes}&anio={vm.Anio}");
                solicitud.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                using HttpResponseMessage respuesta = client.Send(solicitud);

                if (respuesta.StatusCode == HttpStatusCode.Unauthorized)
                {
                    HttpContext.Session.Clear();
                    return RedirectToAction("Index", "Login");
                }

                if (respuesta.StatusCode == HttpStatusCode.Forbidden)
                {
                    vm.MensajeError = "Solo los lectores pueden consultar su historial.";
                    return View(vm);
                }

                if (!respuesta.IsSuccessStatusCode)
                {
                    vm.MensajeError = "No se pudo consultar el historial de lecturas.";
                    return View(vm);
                }

                string json = respuesta.Content.ReadAsStringAsync().Result;
                vm.Lecturas = JsonSerializer.Deserialize<List<DtoLecturaHistorialApi>>(json, _options) ?? new();
                return View(vm);
            }
            catch (HttpRequestException)
            {
                vm.MensajeError = "No se pudo conectar con la API.";
                return View(vm);
            }
            catch (JsonException)
            {
                vm.MensajeError = "La API devolvió un formato de datos inválido.";
                return View(vm);
            }
        }
    }
}
