﻿namespace FrontEnd.Backend
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class BackEndFacade : IBackEndFacade
    {
        public Task CreateCourse(string name, Guid id)
        {
            return null;
        }

        public Task<IReadOnlyCollection<Course>> GetAllCourses()
        {
            return null;
        }

        public Task<Course> GetCourse(Guid id)
        {
            return null;
        }

        public Task CreateStudent(Student student)
        {
            return null;
        }

        public Task<IReadOnlyCollection<Student>> GetStudents(Guid courseId)
        {
            return null;
        }

        public Task<Student> GetNextStudentToLearn(Guid courseId)
        {
            return null;
        }
    }
}