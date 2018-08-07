namespace FrontEnd.Backend
{
    using System;

    public struct Student
    {
        public Student(
            Guid courseId,
            string firstname,
            string lastname,
            string imageInBase64)
        {
            this.CourseId = courseId;
            this.Firstname = firstname;
            this.Lastname = lastname;
            this.ImageInBase64 = imageInBase64;
        }

        public Guid CourseId { get; }

        public string Firstname { get; }

        public string Lastname { get; }

        public string ImageInBase64 { get; }
    }
}