namespace FrontEnd.Backend
{
    using System;
    using System.Threading.Tasks;

    public interface IBackEndFacade
    {
        Task CreateCourse(
            string name,
            Guid id);
    }
}