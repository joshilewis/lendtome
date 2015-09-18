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
        public long Id { get; set; }

        protected Event(long id)
        {
            Id = id;
        }

        protected Event() { }
    }
}
