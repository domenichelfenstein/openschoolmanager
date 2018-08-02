namespace FrontEnd.Fakes
{
    using System;
    using Backend;

    public class FakeGuidGenerator : IGuidGenerator
    {
        private Guid next;

        public void SetNext(
            Guid guid)
        {
            this.next = guid;
        }

        public Guid Generate()
        {
            return this.next;
        }
    }
}