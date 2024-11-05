using CRUDWEB.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

public class MovimientosMVCController : Controller
{
    private readonly string apiUrl = "https://localhost:44396/api/movimientos";

    public ActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public async Task<ActionResult> Consultar(string fechaInicio, string fechaFin, string tipoMovimiento, string nroDocumento)
    {
        List<Movimiento> movimientos = new List<Movimiento>();

        var requestUrl = $"{apiUrl}/consultar?";

        bool hasParams = false;

        if (!string.IsNullOrEmpty(fechaInicio))
        {
            requestUrl += $"fechaInicio={Uri.EscapeDataString(fechaInicio)}";
            hasParams = true;
        }

        if (!string.IsNullOrEmpty(fechaFin))
        {
            if (hasParams) requestUrl += "&";
            requestUrl += $"fechaFin={Uri.EscapeDataString(fechaFin)}";
            hasParams = true;
        }

        if (!string.IsNullOrEmpty(tipoMovimiento))
        {
            if (hasParams) requestUrl += "&";
            requestUrl += $"tipoMovimiento"; 
        }

        if (!string.IsNullOrEmpty(nroDocumento))
        {
            if (hasParams) requestUrl += "&";
            requestUrl += $"nroDocumento={Uri.EscapeDataString(nroDocumento)}";
        }

        if (!hasParams)
        {
            requestUrl = $"{apiUrl}/consultar";
        }

        using (HttpClient client = new HttpClient())
        {
            var response = await client.GetAsync(requestUrl);

            if (response.IsSuccessStatusCode)
            {
                var jsonData = await response.Content.ReadAsStringAsync();
                movimientos = JsonConvert.DeserializeObject<List<Movimiento>>(jsonData);
            }
            else
            {
                ViewBag.Error = "Error al consultar movimientos";
            }
        }

        return View(movimientos);
    }





    public ActionResult Nuevo()
    {
        return View();
    }

    [HttpPost]
    public async Task<ActionResult> Nuevo(ParametroNuevo nuevoMovimiento)
    {
        if (ModelState.IsValid)
        {
            using (HttpClient client = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(nuevoMovimiento);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync($"{apiUrl}/nuevo", content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Consultar");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "No se pudo crear el movimiento.");
                }
            }
        }

        return View(nuevoMovimiento);
    }
}
