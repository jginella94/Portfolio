using Microsoft.AspNetCore.Mvc;
using ObligatorioP3.WebLectura.Models;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace ObligatorioP3.WebLectura.Controllers
{
    public class LecturasController : Controller
    {
        private readonly IHttpClientFactory _factory;
        private readonly JsonSerializerOptions _options;

        public LecturasController(IHttpClientFactory factory, JsonSerializerOptions options)
        {
            _factory = factory;
            _options = options;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Iniciar(int historiaId)
        {
            string? token = HttpContext.Session.GetString("token");
            if (string.IsNullOrWhiteSpace(token))
                return RedirectToAction("Index", "Login");

            if (historiaId <= 0)
            {
                TempData["MensajeError"] = "La historia seleccionada no es válida.";
                return RedirectToAction("Index", "Historias");
            }

            try
            {
                HttpClient client = _factory.CreateClient("Api");
                using HttpRequestMessage solicitud = new(HttpMethod.Post, "api/Lecturas");
                solicitud.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                solicitud.Content = new StringContent(
                    JsonSerializer.Serialize(new { HistoriaId = historiaId }, _options),
                    Encoding.UTF8,
                    "application/json");

                using HttpResponseMessage respuesta = client.Send(solicitud);

                if (respuesta.StatusCode == HttpStatusCode.Unauthorized)
                {
                    HttpContext.Session.Clear();
                    return RedirectToAction("Index", "Login");
                }

                if (respuesta.StatusCode == HttpStatusCode.Forbidden)
                {
                    TempData["MensajeError"] = "Solo los lectores pueden iniciar una lectura.";
                    return RedirectToAction("Index", "Historias");
                }

                if (!respuesta.IsSuccessStatusCode)
                {
                    TempData["MensajeError"] = "No se pudo iniciar la lectura. Verificá que la historia esté publicada.";
                    return RedirectToAction("Index", "Historias");
                }

                string json = respuesta.Content.ReadAsStringAsync().Result;
                DtoLecturaActualApi? lectura = JsonSerializer.Deserialize<DtoLecturaActualApi>(json, _options);

                if (lectura is null || lectura.LecturaId <= 0)
                {
                    TempData["MensajeError"] = "La API devolvió una lectura inválida.";
                    return RedirectToAction("Index", "Historias");
                }

                return RedirectToAction(nameof(Detalle), new { id = lectura.LecturaId });
            }
            catch (HttpRequestException)
            {
                TempData["MensajeError"] = "No se pudo conectar con la API.";
                return RedirectToAction("Index", "Historias");
            }
            catch (JsonException)
            {
                TempData["MensajeError"] = "La API devolvió datos con un formato inválido.";
                return RedirectToAction("Index", "Historias");
            }
        }

        [HttpGet]
        public IActionResult Detalle(int id)
        {
            string? token = HttpContext.Session.GetString("token");
            if (string.IsNullOrWhiteSpace(token))
                return RedirectToAction("Index", "Login");

            if (id <= 0)
                return RedirectToAction("Index", "Historias");

            try
            {
                HttpClient client = _factory.CreateClient("Api");
                using HttpRequestMessage solicitud = new(HttpMethod.Get, $"api/Lecturas/{id}");
                solicitud.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                using HttpResponseMessage respuesta = client.Send(solicitud);

                if (respuesta.StatusCode == HttpStatusCode.Unauthorized)
                {
                    HttpContext.Session.Clear();
                    return RedirectToAction("Index", "Login");
                }

                if (respuesta.StatusCode == HttpStatusCode.Forbidden)
                {
                    TempData["MensajeError"] = "No tenés permiso para consultar lecturas.";
                    return RedirectToAction("Index", "Historias");
                }

                if (respuesta.StatusCode == HttpStatusCode.NotFound)
                {
                    TempData["MensajeError"] = "No se encontró esta lectura o no pertenece al usuario autenticado.";
                    return RedirectToAction("Index", "Historias");
                }

                if (!respuesta.IsSuccessStatusCode)
                {
                    TempData["MensajeError"] = "No se pudo consultar la lectura.";
                    return RedirectToAction("Index", "Historias");
                }

                string json = respuesta.Content.ReadAsStringAsync().Result;
                DtoLecturaActualApi? lectura = JsonSerializer.Deserialize<DtoLecturaActualApi>(json, _options);
                if (lectura is null || lectura.LecturaId != id)
                {
                    TempData["MensajeError"] = "La API devolvió una lectura inválida.";
                    return RedirectToAction("Index", "Historias");
                }

                return View(new VMDetalleLectura { Lectura = lectura });
            }
            catch (HttpRequestException)
            {
                TempData["MensajeError"] = "No se pudo conectar con la API.";
                return RedirectToAction("Index", "Historias");
            }
            catch (JsonException)
            {
                TempData["MensajeError"] = "La API devolvió datos con un formato inválido.";
                return RedirectToAction("Index", "Historias");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Elegir(int id, int opcionId)
        {
            string? token = HttpContext.Session.GetString("token");
            if (string.IsNullOrWhiteSpace(token))
                return RedirectToAction("Index", "Login");

            if (id <= 0 || opcionId <= 0)
            {
                TempData["MensajeError"] = "La opción seleccionada no es válida.";
                return RedirectToAction("Index", "Historias");
            }

            try
            {
                HttpClient client = _factory.CreateClient("Api");
                using HttpRequestMessage solicitud = new(HttpMethod.Post, $"api/Lecturas/{id}/opciones/{opcionId}");
                solicitud.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                using HttpResponseMessage respuesta = client.Send(solicitud);

                if (respuesta.StatusCode == HttpStatusCode.Unauthorized)
                {
                    HttpContext.Session.Clear();
                    return RedirectToAction("Index", "Login");
                }

                if (respuesta.StatusCode == HttpStatusCode.Forbidden)
                {
                    TempData["MensajeError"] = "No tenés permiso para avanzar lecturas.";
                    return RedirectToAction("Index", "Historias");
                }

                if (respuesta.StatusCode == HttpStatusCode.NotFound)
                {
                    TempData["MensajeError"] = "La lectura no existe o no te pertenece.";
                    return RedirectToAction("Index", "Historias");
                }

                if (!respuesta.IsSuccessStatusCode)
                    TempData["MensajeError"] = "No se pudo avanzar. La opción puede no corresponder al capítulo actual o la lectura ya terminó.";

                return RedirectToAction(nameof(Detalle), new { id });
            }
            catch (HttpRequestException)
            {
                TempData["MensajeError"] = "No se pudo conectar con la API.";
                return RedirectToAction(nameof(Detalle), new { id });
            }
        }
    }
}
