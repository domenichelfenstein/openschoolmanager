namespace FrontEnd.Fakes
{
    using System;
    using System.Threading.Tasks;
    using Backend;

    public class FakeBackEndFacade : IBackEndFacade
    {
        public Task CreateCourse(
            string name,
            Guid id)
        {
            this.CreateCourseId = id;
            this.CreateCourseName = name;
            return Task.CompletedTask;
        }

        public string CreateCourseName { get; private set; }

        public Guid CreateCourseId { get; private set; }
    }
}