namespace FrontEnd.Fakes
{
    using System.Threading.Tasks;
    using Backend;

    public class FakeBackEndFacade : IBackEndFacade
    {
        public Task CreateCourse(
            string name)
        {
            this.CreateCourseName = name;
            return Task.CompletedTask;
        }

        public string CreateCourseName { get; private set; }
    }
}