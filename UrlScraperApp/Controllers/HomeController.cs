﻿using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using UrlScraperApp.Models;

namespace UrlScraperApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var model = new UrlContent();
            
            // This was set up for testing and I ran out of time
            // Also did not get the update of content on form submission set up
            // Form will sumit url to get api and get data
            // Can be tested in debug mode by setting breakpoints in get call
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7172/api/");
                //HTTP GET
                var responseTask = client.GetAsync("scraper");
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

            return View(model);
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