using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ObligatorioP3.WebLectura.Models;
using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;

namespace ObligatorioP3.WebLectura.Controllers
{
    public class HistoriasController : Controller
    {
        private readonly IHttpClientFactory _factory;
        private readonly JsonSerializerOptions _options;

        public HistoriasController(IHttpClientFactory factory, JsonSerializerOptions options)
        {
            _factory = factory;
            _options = options;
        }

        [HttpGet]
        public IActionResult Index(int? categoriaId)
        {
            string? token = HttpContext.Session.GetString("token");
            if (string.IsNullOrWhiteSpace(token))
                return RedirectToAction("Index", "Login");

            VMHistorias vm = new VMHistorias { CategoriaId = categoriaId };

            if (categoriaId.HasValue && categoriaId.Value <= 0)
            {
                vm.MensajeError = "La categoría seleccionada no es válida.";
                return View(vm);
            }

            try
            {
                HttpClient client = _factory.CreateClient("Api");

                using HttpRequestMessage solicitudCategorias = new(HttpMethod.Get, "api/Categorias");
                solicitudCategorias.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                using HttpResponseMessage respuestaCategorias = client.Send(solicitudCategorias);

                if (respuestaCategorias.StatusCode == HttpStatusCode.Unauthorized)
                {
                    HttpContext.Session.Clear();
                    return RedirectToAction("Index", "Login");
                }

                if (respuestaCategorias.StatusCode == HttpStatusCode.Forbidden)
                {
                    vm.MensajeError = "El listado de historias está disponible solamente para lectores.";
                    return View(vm);
                }

                if (!respuestaCategorias.IsSuccessStatusCode)
                {
                    vm.MensajeError = "No se pudieron obtener las categorías.";
                    return View(vm);
                }

                string jsonCategorias = respuestaCategorias.Content.ReadAsStringAsync().Result;
                List<DtoCategoriaApi> categorias = JsonSerializer.Deserialize<List<DtoCategoriaApi>>(jsonCategorias, _options) ?? new();
                vm.CategoriasDisponibles = categorias
                    .OrderBy(c => c.Nombre)
                    .Select(c => new SelectListItem
                    {
                        Value = c.Id.ToString(),
                        Text = c.Nombre
                    })
                    .ToList();

                string ruta = "api/HistoriasPublicadas";
                if (categoriaId.HasValue)
                    ruta += "?categoriaId=" + categoriaId.Value;

                using HttpRequestMessage solicitudHistorias = new(HttpMethod.Get, ruta);
                solicitudHistorias.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                using HttpResponseMessage respuestaHistorias = client.Send(solicitudHistorias);

                if (respuestaHistorias.StatusCode == HttpStatusCode.Unauthorized)
                {
                    HttpContext.Session.Clear();
                    return RedirectToAction("Index", "Login");
                }

                if (respuestaHistorias.StatusCode == HttpStatusCode.Forbidden)
                {
                    vm.MensajeError = "No tenés permisos para consultar las historias.";
                    return View(vm);
                }

                if (!respuestaHistorias.IsSuccessStatusCode)
                {
                    vm.MensajeError = "No se pudo obtener el listado de historias.";
                    return View(vm);
                }

                string jsonHistorias = respuestaHistorias.Content.ReadAsStringAsync().Result;
                vm.Historias = JsonSerializer.Deserialize<List<DtoHistoriaPublicadaApi>>(jsonHistorias, _options) ?? new();
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
