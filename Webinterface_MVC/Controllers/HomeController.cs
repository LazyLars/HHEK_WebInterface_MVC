using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Webinterface_MVC.Models;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;

namespace Webinterface_MVC.Controllers
{
    public class HomeController : Controller
    {
        public JsonString totalString;

        public HomeController()
        {
            totalString = new JsonString();
            totalString.Content = "";
        }

        public IActionResult Index()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("http://Ci-slave1.virtapi.org:9494");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            //client.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");

            HttpResponseMessage response = client.GetAsync("/json").Result;
            if (response.IsSuccessStatusCode)
            {
                totalString.Content = response.Content.ReadAsStringAsync().Result;
            }
            return View("Index",totalString);
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> Post(JsonString model)
        {
            if(String.IsNullOrEmpty(model.Content))
            {
                ViewBag.Result = "Textfeld ist leer...Dann geht das nicht!";
                return View("Index", model.Content);
            }
            var client = new HttpClient();
            client.BaseAddress = new Uri("http://Ci-slave1.virtapi.org:9494");

            var response = client.PostAsync("/json", new StringContent(model.Content, Encoding.UTF8, "application/json")).Result;
            
            if (response.IsSuccessStatusCode)
            {
                ViewBag.Result = "Erfolgreiches Schreiben der Hashtags! Ruft die Liste ab zum überprüfen.";
                totalString.Content = response.Content.ReadAsStringAsync().Result;
            }
            else
            {
                ViewBag.Result = "Etwas ist Fehlgeschlagen! ";

            }
            return View("Index", totalString);
        }

        public async Task<IActionResult> Get()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("http://Ci-slave1.virtapi.org:9494");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            //client.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");

            HttpResponseMessage response = client.GetAsync("/json").Result;
            if (response.IsSuccessStatusCode)
            {
                totalString.Content = response.Content.ReadAsStringAsync().Result;
            }
            return View("Index", totalString);
        }

        public IActionResult Delete()
        {

            var client = new HttpClient();
            client.BaseAddress = new Uri("http://Ci-slave1.virtapi.org:9494");

            var response = client.DeleteAsync("/json").Result;

            if (response.IsSuccessStatusCode)
            {
                ViewBag.Result = "Erfolgreiches Löschen der Hashtags! Ruft die Liste ab zum überprüfen.";
                totalString.Content = response.Content.ReadAsStringAsync().Result;
            }
            else
            {
                ViewBag.Result = "Etwas ist Fehlgeschlagen! ";

            }
            return View("Index", totalString);
        }
    }

    public class JsonString
    {
        public string Content { get; set; }
    }
}
