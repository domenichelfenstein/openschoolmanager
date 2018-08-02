namespace FrontEnd.Backend
{
    using System;

    public class GuidGenerator : IGuidGenerator
    {
        public Guid Generate()
            => Guid.NewGuid();
    }
}