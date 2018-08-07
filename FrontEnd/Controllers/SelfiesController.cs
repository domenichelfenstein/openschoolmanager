namespace FrontEnd.Controllers
{
    using System.Threading.Tasks;
    using Backend;
    using Microsoft.AspNetCore.Mvc;

    [Produces("application/json")]
    [Route("api/selfies")]
    public class SelfiesController : Controller
    {
        private readonly IBackEndFacade backend;

        public SelfiesController(
            IBackEndFacade backend)
        {
            this.backend = backend;
        }

        [HttpPost]
        public async Task<IActionResult> Post(
            [FromBody]Student student)
        {
            await this.backend.CreateStudent(
                student);

            return this.Ok();
        }
    }
}