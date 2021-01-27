using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace TesteAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RepositoriesController : ControllerBase
    {
        [HttpGet]
        public async Task<string> Get()
        {
            var response = await PopulateCarousel();
            return response;
        }       

        private async Task<string> PopulateCarousel()
        {
            List<Rootobject> Repos = await FilterRepos();
            Carousel carousel = new Carousel();

            carousel.itemType = "application/vnd.lime.document-select+json";
            carousel.items = new List<Header>();


            Repos.ForEach(repos =>
            {
                HeaderBody headerBody = new HeaderBody();
                Header header = new Header();
                header.header = new HeaderBody();

                headerBody.value = new Value();

                headerBody.type = "application/vnd.lime.media-link+json";

                headerBody.value.title = repos.full_name;
                headerBody.value.text = repos.description;
                headerBody.value.type = "image/jpeg";
                headerBody.value.uri = repos.owner.avatar_url;

                header.header = headerBody;
                carousel.items.Add(header);
            });



            string response = JsonConvert.SerializeObject(carousel);

            return response;
        }

        private async Task<List<Rootobject>> FilterRepos()
        {
            List<Rootobject> allRepos = new List<Rootobject>();
            var jsonRepos = await ConsumirDados();
            JsonConvert.PopulateObject(jsonRepos, allRepos);
            var Filter = allRepos.Where(x => x.language == "C#").OrderBy(x => x.created_at).Take(5).ToList();

            return Filter;
        }

        private static async Task<string> ConsumirDados()
        {
            HttpClient client = new HttpClient();

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
            client.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");

            var stringTask = client.GetStringAsync("https://api.github.com/orgs/takenet/repos");
            var response = stringTask;
            await response;

            return response.Result;
        }
    }
}
