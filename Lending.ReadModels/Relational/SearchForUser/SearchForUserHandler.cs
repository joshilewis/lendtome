using System;
using System.Linq;
using Lending.Cqrs.Command;
using Lending.Cqrs.Query;
using Lending.Domain.RegisterUser;
using NHibernate;

namespace Lending.ReadModels.Relational.SearchForUser
{
    public class SearchForUserHandler : IQueryHandler<SearchForUser, Result>
    {
        private readonly Func<ISession> getSession;

        public SearchForUserHandler(Func<ISession> sessionFunc)
        {
            this.getSession = sessionFunc;
        }

        public Result Handle(SearchForUser query)
        {
            RegisteredUser[] users = getSession().QueryOver<RegisteredUser>()
                .WhereRestrictionOn(x => x.UserName).IsInsensitiveLike("%" + query.SearchString.ToLower() + "%")
                .List()
                .ToArray();

            return new Result<RegisteredUser[]>(users);
        }
    }
}