using System;

namespace Lending.Domain
{
    /// <summary>
    /// Marker class used for type resolution
    /// </summary>
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
