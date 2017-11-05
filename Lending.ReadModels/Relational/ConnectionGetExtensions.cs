using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Lending.ReadModels.Relational.LibraryOpened;

namespace Lending.ReadModels.Relational
{
    public static class ConnectionGetExtensions
    {
        public static AuthenticatedUser GetAuthenticatedUser(this IDbConnection connection, Guid id)
        {
            return connection
                .Query<AuthenticatedUser>("select *  from \"AuthenticatedUser\" where \"Id\" = @id", new {id})
                .SingleOrDefault();
        }

        public static OpenedLibrary GetOpenedLibrary(this IDbConnection connection, Guid id)
        {
            return connection
                .Query<OpenedLibrary>("select *  from \"OpenedLibrary\" where \"Id\" = @id", new {id })
                .SingleOrDefault();
        }

    }
}
