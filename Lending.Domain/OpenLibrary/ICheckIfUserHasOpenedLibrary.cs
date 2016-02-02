using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lending.Domain.OpenLibrary
{
    public interface ICheckIfUserHasOpenedLibrary
    {
        bool UserHasOpenedLibrary(Guid userId);
    }
}
