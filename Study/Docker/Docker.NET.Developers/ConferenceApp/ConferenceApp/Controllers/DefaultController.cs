using ConferenceApp.Data;
using Microsoft.AspNetCore.Mvc;

namespace ConferenceApp.Controllers
{
    [Produces("application/json")]
    [Route("api")]
    public class DefaultController : Controller
    {
        private readonly ConferenceContext context;
        private readonly string appHref;

        public DefaultController(ConferenceContext context)
        {
            this.context = context;
            appHref = "http://localhost:5000/api";
        }

        [HttpGet]
        public IActionResult Get()
        {
            var rootData = new
            {
                Api = "Conference API",
                Version = 1,
                Href = this.appHref,
                Speakers = new
                {
                    Href = $"{this.appHref}/speakers"
                }
            };
            return Ok(rootData);
        }
    }
}