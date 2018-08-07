namespace FrontEnd.Backend
{
    using System;

    public struct Course
    {
        public Course(
            Guid id,
            string name)
        {
            this.Id = id;
            this.Name = name;
        }

        public Guid Id { get; }

        public string Name { get; }
    }
}