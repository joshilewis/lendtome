using System;
using System.Linq;
using Lending.Cqrs.Command;
using Lending.Cqrs.Query;
using Lending.Domain.RegisterUser;
using NHibernate;

namespace Tests
{
    public class SearchForUserHandler : IQueryHandler<SearchForUser, RegisteredUser[]>
    {
        private readonly Func<ISession> getSession;

        public SearchForUserHandler(Func<ISession> sessionFunc)
        {
            this.getSession = sessionFunc;
        }

        public Result<RegisteredUser[]> HandleQuery(SearchForUser query)
        {
            RegisteredUser[] users = getSession().QueryOver<RegisteredUser>()
                .WhereRestrictionOn(x => x.UserName).IsInsensitiveLike("%" + query.SearchString.ToLower() + "%")
                .List()
                .ToArray();

            return new Result<RegisteredUser[]>(users);
        }
    }
}