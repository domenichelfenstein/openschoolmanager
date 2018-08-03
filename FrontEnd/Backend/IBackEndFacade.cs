namespace FrontEnd.Backend
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IBackEndFacade
    {
        Task CreateCourse(
            string name,
            Guid id);

        Task<IReadOnlyCollection<Course>> GetAllCourses();
    }
}