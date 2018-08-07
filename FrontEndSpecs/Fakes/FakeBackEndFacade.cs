namespace FrontEnd.Fakes
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Backend;

    public class FakeBackEndFacade : IBackEndFacade
    {
        private Action<string> onAddCourse = _ => { };

        public Task CreateCourse(
            string name,
            Guid id)
        {
            this.CreateCourseId = id;
            this.CreateCourseName = name;

            this.onAddCourse(name);
            return Task.CompletedTask;
        }

        public Task<IReadOnlyCollection<Course>> GetAllCourses()
        {
            return Task.FromResult<IReadOnlyCollection<Course>>(
                this.Courses);
        }

        public Task<Course> GetCourse(Guid id)
        {
            this.GetCourseId = id;
            return Task.FromResult(this.Course);
        }

        public string CreateCourseName { get; private set; }

        public Guid CreateCourseId { get; private set; }

        public Course[] Courses { get; set; }

        public Course Course { get; set; }

        public Guid GetCourseId { get; private set; }

        public void OnAddCourse(Action<string> func)
        {
            this.onAddCourse = func;
        }
    }
}