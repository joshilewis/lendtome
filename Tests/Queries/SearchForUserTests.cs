using System;
using Lending.Cqrs.Query;
using Lending.Domain.RegisterUser;
using Lending.ReadModels.Relational.SearchForUser;
using NUnit.Framework;

namespace Tests.Queries
{
    [TestFixture]
    public class SearchForUserTests: FixtureWithEventStoreAndNHibernate
    {
        #region Fields
        private Guid processId = Guid.Empty;
        private Guid user1Id;
        private Guid user2Id;
        private Guid user3Id;
        private Guid user4Id;

        //Events
        private UserRegistered user1Registered;
        private UserRegistered user2Registered;
        private UserRegistered user3Registered;
        private UserRegistered user4Registered;

        public override void SetUp()
        {
            base.SetUp();
            user1Id = Guid.NewGuid();
            user2Id = Guid.NewGuid();
            user3Id = Guid.NewGuid();
            user4Id = Guid.NewGuid();
            processId = Guid.NewGuid();

            user1Registered = new UserRegistered(processId, user1Id, 1, "Joshua Lewis", "Email1");
            user2Registered = new UserRegistered(processId, user2Id, 2, "Suzaan Hepburn", "Email2");
            user3Registered = new UserRegistered(processId, user3Id, 3, "Josie Doe", "Email3");
            user4Registered = new UserRegistered(processId, user4Id, 4, "Audrey Hepburn", "Email4");
        }

        #endregion

        /// <summary>
        /// GIVEN Users with the following names 'Joshua Lewis', 'Suzaan Hepburn', 'Joshua Doe', 'Audrey Hepburn' have Registered 
        /// WHEN I Search for Users with the search string 'Lew' 
        /// THEN 'Joshua Lewis' gets returne
        /// </summary>
        [Test]
        public void SearchingForUserWithSingleMatchShouldReturnThatUser()
        {
            Given(user1Registered, user2Registered, user3Registered, user4Registered);
            When(new SearchForUser("Lew"));
            Then(actualResult => ((Result<RegisteredUser[]>)actualResult).ShouldEqual(new Result<RegisteredUser[]>(new RegisteredUser[]
            {
                new RegisteredUser(1, user1Id, user1Registered.UserName), 
            })));
        }

        /// <summary>
        /// GIVEN Users with the following names 'Joshua Lewis', 'Suzaan Hepburn', 'Joshua Doe', 'Audrey Hepburn' have Registered 
        /// WHEN I Search for Users with the search string 'lEw' 
        /// THEN 'Joshua Lewis' gets returne
        /// </summary>
        [Test]
        public void SearchingForUserWithSingleMatchWithWrongCaseShouldReturnThatUser()
        {
            Given(user1Registered, user2Registered, user3Registered, user4Registered);
            When(new SearchForUser("lEw"));
            Then(actualResult => ((Result<RegisteredUser[]>)actualResult).ShouldEqual(new Result<RegisteredUser[]>(new RegisteredUser[]
            {
                new RegisteredUser(1, user1Id, user1Registered.UserName),
            })));
        }

        /// <summary>
        /// GIVEN Users with the following names 'Joshua Lewis', 'Suzaan Hepburn', 'Joshua Doe', 'Audrey Hepburn' have Registered 
        /// WHEN I Search for Users with the search string 'Pet'
        /// THEN 'Joshua Lewis' gets returne
        /// </summary>
        [Test]
        public void SearchingForUserWithNoMatchesShouldReturnEmptyList()
        {
            Given(user1Registered, user2Registered, user3Registered, user4Registered);
            When(new SearchForUser("Pet"));
            Then(actualResult => ((Result<RegisteredUser[]>)actualResult).ShouldEqual(new Result<RegisteredUser[]>(new RegisteredUser[]
            {
            })));

        }

        /// <summary>
        /// GIVEN Users with the following names 'Joshua Lewis', 'Suzaan Hepburn', 'Joshua Doe', 'Audrey Hepburn' have Registered 
        /// WHEN I Search for Users with the search string 'Jos'
        /// THEN 'Joshua Lewis' gets returne
        /// </summary>
        [Test]
        public void SearchingForUserWithTwoMatchsShouldReturnTwoUsers()
        {
            Given(user1Registered, user2Registered, user3Registered, user4Registered);
            When(new SearchForUser("Jos"));
            Then(actualResult => ((Result<RegisteredUser[]>)actualResult).ShouldEqual(new Result<RegisteredUser[]>(new RegisteredUser[]
            {
                new RegisteredUser(1, user1Id, user1Registered.UserName),
                new RegisteredUser(3, user3Id, user3Registered.UserName),
            })));
        }

    }
}
