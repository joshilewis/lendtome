using System;

namespace Lending.Domain
{
    public class StreamEventTuple : Tuple<string, Event>
    {
        public StreamEventTuple(string item1, Event item2)
            : base(item1, item2)
        { }

        public string Stream { get { return Item1; } }
        public Event Event { get { return Item2; } }
    }
}
