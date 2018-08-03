
namespace FrontEnd.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Backend;
    using Microsoft.AspNetCore.Mvc;
    
    [Produces("application/json")]
    [Route("api/courses")]
    public class CoursesController : Controller
    {
        private readonly IBackEndFacade backend;
        private readonly IGuidGenerator guidGenerator;

        public CoursesController(
            IBackEndFacade backend,
            IGuidGenerator guidGenerator)
        {
            this.backend = backend;
            this.guidGenerator = guidGenerator;
        }

        [HttpPost]
        public async Task<IActionResult> Post(
            [FromBody]Create create)
        {
            await this.backend.CreateCourse(
                create.Name,
                this.guidGenerator.Generate());
            return this.Ok();
        }

        [HttpGet]
        public async Task<IReadOnlyCollection<Course>> GetAll()
        {
            var courses = await this.backend.GetAllCourses();

            return courses;
        }

        public struct Create
        {
            public Create(string name)
            {
                this.Name = name;
            }

            public string Name { get; }
        }
    }
}