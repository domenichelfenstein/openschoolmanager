
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

        [HttpGet]
        [Route("{id}")]
        public async Task<Course> Get(
            [FromRoute]Guid id)
        {
            return await this.backend.GetCourse(
                id);
        }

        

        [HttpGet]
        [Route("{id}/students")]
        public async Task<StudentsInCourse> GetStudents(
            [FromRoute]Guid id)
        {
            var course = await this.backend.GetCourse(
                id);

            var students = await this.backend.GetStudents(
                id);

            return new StudentsInCourse(
                    course.Id,
                    course.Name,
                    students);
        }

        public struct Create
        {
            public Create(string name)
            {
                this.Name = name;
            }

            public string Name { get; }
        }

        public struct StudentsInCourse
        {
            public StudentsInCourse(Guid courseId, string courseName, IReadOnlyCollection<Student> students)
            {
                this.CourseId = courseId;
                this.CourseName = courseName;
                this.Students = students;
            }

            public Guid CourseId { get; }

            public string CourseName { get; }

            public IReadOnlyCollection<Student> Students { get; }
        }
    }
}