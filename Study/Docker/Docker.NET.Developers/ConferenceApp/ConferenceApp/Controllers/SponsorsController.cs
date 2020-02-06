using System.Threading.Tasks;
using ConferenceApp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ConferenceApp.Controllers
{
    [Produces("application/json")]
    [Route("api/sponsors")]
    public class SponsorsController : Controller
    {
        private readonly ConferenceContext context;

        public SponsorsController(ConferenceContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await context.Sponsors.ToListAsync());
        }
    }
}