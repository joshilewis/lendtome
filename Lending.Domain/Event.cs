using System;

namespace Lending.Domain
{
    public abstract class Event
    {
        public Guid Id { get; set; }

        protected Event(Guid id)
        {
            Id = id;
        }

        protected Event() { }
    }
}
