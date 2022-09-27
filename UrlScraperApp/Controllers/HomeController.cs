using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using UrlScraperApp.Models;
using UrlScraperApp.Services;

namespace UrlScraperApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IViewToStringRenderer _viewToStringRenderer;

        public HomeController(ILogger<HomeController> logger, IViewToStringRenderer viewToStringRenderer)
        {
            _logger = logger;
            _viewToStringRenderer = viewToStringRenderer;
        }

        public IActionResult Index()
        {
            var model = new UrlContent();

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> UpdateContent(string url)
        {
            var model = new UrlContent();

            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://localhost:7172/api/");
                    
                    //HTTP GET call to api
                    var responseTask = client.GetAsync("LoadUrl?url=" + url);
                    responseTask.Wait();

                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var readTask = result.Content.ReadFromJsonAsync<UrlContent>();
                        readTask.Wait();

                        model = readTask.Result;
                    }
                    else //web api sent error response 
                    {
                        ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                    }
                }

                var partialString = await _viewToStringRenderer.RenderViewToString("/Views/Home/_ScrapedContent.cshtml", model, true);

                return Json(new { success = true, payload = partialString });
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);

                throw;
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}