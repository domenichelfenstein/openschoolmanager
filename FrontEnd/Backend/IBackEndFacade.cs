namespace FrontEnd.Backend
{
    using System.Threading.Tasks;

    public interface IBackEndFacade
    {
        Task CreateCourse(
            string name);
    }
}