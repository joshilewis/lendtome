using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lending.Cqrs.Query
{
    public interface IAuthenticatedQueryHandler<in TQuery, out TResult> : IQueryHandler<TQuery, TResult>
        where TQuery : Query, IAuthenticated where TResult : Result
    {
    }
}
