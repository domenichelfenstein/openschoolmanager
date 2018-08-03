namespace FrontEnd.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    public class HomeController : Controller
    {
        [Route("/")]
        [HttpGet]
        public string Index()
        {
            return "hello!";
        }
    }
}