using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lending.Core.Model;
using Lending.Execution.Auth;
using ServiceStack.Authentication.NHibernate;
using ServiceStack.ServiceInterface.Auth;

namespace Tests
{
    public class DefaultTestData
    {
        public static UserAuth UserAuth1
        {
            get
            {
                return new UserAuth()
                {
                    DisplayName = "Joshua Lewis",
                    PrimaryEmail = ""
                };
            }
        }

        public static UserAuthPersistenceDto UserAuthPersistenceDto1
        {
            get { return new UserAuthPersistenceDto(UserAuth1); }
        }

        public static User ServiceStackUser1
        {
            get { return new ServiceStackUser(UserAuthPersistenceDto1); }
        }

        public static UserAuth UserAuth2
        {
            get
            {
                return new UserAuth()
                {
                    DisplayName = "User2",
                    PrimaryEmail = "email 2"
                };
            }
        }

        public static UserAuthPersistenceDto UserAuthPersistenceDto2
        {
            get { return new UserAuthPersistenceDto(UserAuth2); }
        }

        public static User ServiceStackUser2
        {
            get { return new ServiceStackUser(UserAuthPersistenceDto2); }
        }

        public static UserAuth UserAuth3
        {
            get
            {
                return new UserAuth()
                {
                    DisplayName = "User3",
                    PrimaryEmail = "email 3"
                };
            }
        }

        public static UserAuthPersistenceDto UserAuthPersistenceDto3
        {
            get { return new UserAuthPersistenceDto(UserAuth3); }
        }

        public static User ServiceStackUser3
        {
            get { return new ServiceStackUser(UserAuthPersistenceDto3); }
        }

    }
}
