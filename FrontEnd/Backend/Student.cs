namespace FrontEnd.Backend
{
    using System;

    public struct Student
    {
        public Student(
            Guid courseId,
            string studentFirstname,
            string stundetLastname,
            string imageInBase64)
        {
            this.CourseId = courseId;
            this.StudentFirstname = studentFirstname;
            this.StundetLastname = stundetLastname;
            this.ImageInBase64 = imageInBase64;
        }

        public Guid CourseId { get; }

        public string StudentFirstname { get; }

        public string StundetLastname { get; }

        public string ImageInBase64 { get; }
    }
}