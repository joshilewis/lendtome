using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lending.Cqrs
{
    public interface IAuthenticated
    {
        Guid UserId { get; }
    }
}
