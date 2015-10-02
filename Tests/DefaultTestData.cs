using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lending.Execution.Auth;
using ServiceStack.Authentication.NHibernate;
using ServiceStack.ServiceInterface.Auth;

namespace Tests
{
    public class DefaultTestData
    {
        public static UserAuth UserAuth1 => new UserAuth()
        {
            DisplayName = "Joshua Lewis",
            PrimaryEmail = ""
        };

        public static UserAuthPersistenceDto UserAuthPersistenceDto1 => new UserAuthPersistenceDto(UserAuth1);

        public static ServiceStackUser ServiceStackUser1 => new ServiceStackUser(UserAuthPersistenceDto1, Guid.Empty);

        public static UserAuth UserAuth2 => new UserAuth()
        {
            DisplayName = "User2",
            PrimaryEmail = "email 2"
        };

        public static UserAuthPersistenceDto UserAuthPersistenceDto2 => new UserAuthPersistenceDto(UserAuth2);

        public static ServiceStackUser ServiceStackUser2 => new ServiceStackUser(UserAuthPersistenceDto2, Guid.Empty);

        public static UserAuth UserAuth3 => new UserAuth()
        {
            DisplayName = "User3",
            PrimaryEmail = "email 3"
        };

        public static UserAuthPersistenceDto UserAuthPersistenceDto3 => new UserAuthPersistenceDto(UserAuth3);

        public static ServiceStackUser ServiceStackUser3 => new ServiceStackUser(UserAuthPersistenceDto3, Guid.Empty);
    }
}
