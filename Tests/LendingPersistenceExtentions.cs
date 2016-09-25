using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Joshilewis.Testing.Helpers;
using Lending.ReadModels.Relational;

namespace Tests
{
    public class LendingPersistenceExtentions
    {
        public static void UserRegisters(Guid id, string name, string emailAddress, string picture)
        {
            PersistenceExtensions.OpenTransaction();
            PersistenceExtensions.Repository.Save(new AuthenticatedUser(id, name, emailAddress, picture, new List<AuthenticationProvider>()));
            PersistenceExtensions.CommitTransaction();
        }

    }
}
