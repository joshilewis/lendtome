using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lending.Core
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
