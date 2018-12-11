using AdminApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace AdminApp.Controllers
{
    public class EventsController : Controller
    {
        // GET: Events
        public ActionResult Index()
        {
            return View();
        }

        // Method for posting events to the visitor application web api
        // This is used when the form in Event folder > Index sends data to it
        [HttpPost]
        public async Task<HttpResponseMessage> PostEvents(Event events)
        {
            HttpClient client = new HttpClient();
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("title", events.Title), // Needs to be changed
                new KeyValuePair<string, string>("bodyText", events.BodyText) // Needs to be changed
            });
            content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
            HttpResponseMessage response = await client.PostAsync("http://localhost:49880/umbraco/api/utest/postnews", content); // Needs to be changed
            Response.Redirect("/Events");

            return response;
        }


        [HttpPost]
        public async Task<HttpResponseMessage> DeleteEvent(Event events)
        {
            HttpClient client = new HttpClient();
            //var id = news.Id.ToString();
            var identifier = events.Id.ToString(); // Needs to be changed
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("id", identifier)
            });
            content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
            HttpResponseMessage response = await client.PostAsync("http://localhost:49880/umbraco/api/utest/deletenews", content); // Needs to be changed
            Response.Redirect("/Events");

            return response;
        }


        // Method for getting all the news from visitor application web api
        // This is used when navigating to the "Read News" page (GetNews)
        [HttpGet]
        public ActionResult GetEvents()
        {
            IEnumerable<Event> events = null;

            using (var client = new HttpClient())
            {
                // Url for the api in the visitors application
                client.BaseAddress = new Uri("http://localhost:49880/umbraco/api/utest/news"); // Needs to be changed
                //HTTP GET
                var responseTask = client.GetAsync("news"); // Needs to be changed
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    // Error here if Microsoft.AspNet.WebApi.Client is not installed
                    var readTask = result.Content.ReadAsAsync<IList<Event>>();
                    readTask.Wait();

                    events = readTask.Result;
                }
                else //web api sent error response 
                {
                    //log response status here..

                    events = Enumerable.Empty<Event>();

                    ModelState.AddModelError(string.Empty, "Error");
                }
            }
            return PartialView(@"~/Views/Events/EventsList.cshtml", events);
        }
    }
}