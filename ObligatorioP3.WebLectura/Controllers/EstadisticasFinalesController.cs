using Microsoft.AspNetCore.Mvc;
using ObligatorioP3.WebLectura.Models;
using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;

namespace ObligatorioP3.WebLectura.Controllers
{
    public class EstadisticasFinalesController : Controller
    {
        private readonly IHttpClientFactory _factory;
        private readonly JsonSerializerOptions _options;

        public EstadisticasFinalesController(IHttpClientFactory factory, JsonSerializerOptions options)
        {
            _factory = factory;
            _options = options;
        }

        [HttpGet]
        public IActionResult Index(int? historiaId)
        {
            string? token = HttpContext.Session.GetString("token");
            if (string.IsNullOrWhiteSpace(token))
                return RedirectToAction("Index", "Login");

            VMEstadisticasFinales vm = new() { HistoriaId = historiaId };

            if (historiaId.HasValue && historiaId.Value <= 0)
            {
                vm.MensajeError = "Seleccioná una historia válida.";
                return View(vm);
            }

            try
            {
                HttpClient client = _factory.CreateClient("Api");
                using HttpRequestMessage solicitudHistorias = new(HttpMethod.Get, "api/EstadisticasFinales/historias");
                solicitudHistorias.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                using HttpResponseMessage respuestaHistorias = client.Send(solicitudHistorias);

                if (respuestaHistorias.StatusCode == HttpStatusCode.Unauthorized)
                {
                    HttpContext.Session.Clear();
                    return RedirectToAction("Index", "Login");
                }

                if (respuestaHistorias.StatusCode == HttpStatusCode.Forbidden)
                {
                    vm.MensajeError = "Solo los administradores pueden consultar las estadísticas de finales.";
                    return View(vm);
                }

                if (!respuestaHistorias.IsSuccessStatusCode)
                {
                    vm.MensajeError = "No se pudo obtener el listado de historias.";
                    return View(vm);
                }

                string jsonHistorias = respuestaHistorias.Content.ReadAsStringAsync().Result;
                vm.Historias = JsonSerializer.Deserialize<List<DtoHistoriaParaEstadisticasApi>>(jsonHistorias, _options) ?? new();

                if (!historiaId.HasValue)
                    return View(vm);

                if (!vm.Historias.Any(h => h.HistoriaId == historiaId.Value))
                {
                    vm.MensajeError = "La historia seleccionada no existe.";
                    return View(vm);
                }

                using HttpRequestMessage solicitudEstadisticas = new(HttpMethod.Get, $"api/EstadisticasFinales/{historiaId.Value}");
                solicitudEstadisticas.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                using HttpResponseMessage respuestaEstadisticas = client.Send(solicitudEstadisticas);

                if (respuestaEstadisticas.StatusCode == HttpStatusCode.Unauthorized)
                {
                    HttpContext.Session.Clear();
                    return RedirectToAction("Index", "Login");
                }

                if (respuestaEstadisticas.StatusCode == HttpStatusCode.Forbidden)
                {
                    vm.MensajeError = "No tenés permisos para consultar estas estadísticas.";
                    return View(vm);
                }

                if (respuestaEstadisticas.StatusCode == HttpStatusCode.NotFound)
                {
                    vm.MensajeError = "La historia seleccionada no existe.";
                    return View(vm);
                }

                if (!respuestaEstadisticas.IsSuccessStatusCode)
                {
                    vm.MensajeError = "No se pudieron consultar las estadísticas de la historia.";
                    return View(vm);
                }

                string jsonEstadisticas = respuestaEstadisticas.Content.ReadAsStringAsync().Result;
                vm.Estadisticas = JsonSerializer.Deserialize<DtoEstadisticasFinalesApi>(jsonEstadisticas, _options);
                if (vm.Estadisticas is null)
                    vm.MensajeError = "La API devolvió estadísticas inválidas.";

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
