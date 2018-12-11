using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AdminApp.Models;

namespace AdminApp.Controllers
{
    public class NewsController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        // Method for posting news to the visitor application web api
        // This is used when the form in News folder > Index sends data to it
        [HttpPost]
        public async Task<HttpResponseMessage> PostNews(News news)
        {
            HttpClient client = new HttpClient();
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("title", news.Title),
                new KeyValuePair<string, string>("bodyText", news.BodyText)
            });
            content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
            HttpResponseMessage response =  await client.PostAsync("http://localhost:49880/umbraco/api/utest/postnews", content);
            Response.Redirect("/News");

            return response;
        }

        [HttpPost]
        public async Task<HttpResponseMessage> DeleteNews(News news)
        {
            HttpClient client = new HttpClient();
            //var id = news.Id.ToString();
            var identifier = news.Id.ToString();
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("id", identifier)
            });
            content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
            HttpResponseMessage response = await client.PostAsync("http://localhost:49880/umbraco/api/utest/deletenews", content);
            Response.Redirect("/News");

            return response;
        }


        // Method for getting all the news from visitor application web api
        // This is used when navigating to the "Read News" page (GetNews)
        [HttpGet]
        public ActionResult GetNews()
        {
            IEnumerable<News> news = null;

            using (var client = new HttpClient())
            {
                // Url for the api in the visitors application
                client.BaseAddress = new Uri("http://localhost:49880/umbraco/api/utest/news");
                //HTTP GET
                var responseTask = client.GetAsync("news");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    // Error here if Microsoft.AspNet.WebApi.Client is not installed
                    var readTask = result.Content.ReadAsAsync<IList<News>>();
                    readTask.Wait();

                    news = readTask.Result;
                }
                else //web api sent error response 
                {
                    //log response status here..

                    news = Enumerable.Empty<News>();

                    ModelState.AddModelError(string.Empty, "Error");
                }
            }
            return PartialView(@"~/Views/News/NewsList.cshtml", news);
        }
    }
}