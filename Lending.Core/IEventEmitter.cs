using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lending.Core
{
    public interface IEventEmitter
    {
        void EmitEvent(string stream, Event @event);
    }
}
