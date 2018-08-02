namespace FrontEnd
{
    using System;
    using System.Threading.Tasks;

    public interface IBackEndFacade
    {
        Task<Guid> CreateCourse(
            string name);
    }
}