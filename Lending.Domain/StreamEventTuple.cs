using System;

namespace Lending.Domain
{
    public class StreamEventTuple : Tuple<string, Event>
    {
        public StreamEventTuple(string stream, Event @event)
            : base(stream, @event)
        { }

        public string Stream => Item1;
        public Event Event => Item2;
    }
}
