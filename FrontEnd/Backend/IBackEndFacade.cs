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
        
        Task<Course> GetCourse(Guid id);

        Task CreateStudent(
            Student student);

        Task<IReadOnlyCollection<Student>> GetStudents(
            Guid courseId);

        Task<Student> GetNextStudentToLearn(
            Guid courseId);
    }
}