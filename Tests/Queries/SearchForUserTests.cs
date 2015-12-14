using System;
using Lending.Cqrs.Query;
using Lending.Domain.RegisterUser;
using Lending.ReadModels.Relational.SearchForUser;
using NUnit.Framework;
using static Tests.DefaultTestData;

namespace Tests.Queries
{
    [TestFixture]
    public class SearchForUserTests: FixtureWithEventStoreAndNHibernate
    {

        /// <summary>
        /// GIVEN Users with the following names 'Joshua Lewis', 'Suzaan Hepburn', 'Joshua Doe', 'Audrey Hepburn' have Registered 
        /// WHEN I Search for Users with the search string 'Lew' 
        /// THEN 'Joshua Lewis' gets returne
        /// </summary>
        [Test]
        public void SearchingForUserWithSingleMatchShouldReturnThatUser()
        {
            Given(joshuaLewisRegistered, suzaanHepburnRegistered, josieDoe3Registered, audreyHepburn4Registered);
            When(new SearchForUser("Lew"));
            Then(actualResult => ((Result<RegisteredUser[]>)actualResult).ShouldEqual(new Result<RegisteredUser[]>(new RegisteredUser[]
            {
                new RegisteredUser(1, user1Id, joshuaLewisRegistered.UserName), 
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
            Given(joshuaLewisRegistered, suzaanHepburnRegistered, josieDoe3Registered, audreyHepburn4Registered);
            When(new SearchForUser("lEw"));
            Then(actualResult => ((Result<RegisteredUser[]>)actualResult).ShouldEqual(new Result<RegisteredUser[]>(new RegisteredUser[]
            {
                new RegisteredUser(1, user1Id, joshuaLewisRegistered.UserName),
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
            Given(joshuaLewisRegistered, suzaanHepburnRegistered, josieDoe3Registered, audreyHepburn4Registered);
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
            Given(joshuaLewisRegistered, suzaanHepburnRegistered, josieDoe3Registered, audreyHepburn4Registered);
            When(new SearchForUser("Jos"));
            Then(actualResult => ((Result<RegisteredUser[]>)actualResult).ShouldEqual(new Result<RegisteredUser[]>(new RegisteredUser[]
            {
                new RegisteredUser(1, user1Id, joshuaLewisRegistered.UserName),
                new RegisteredUser(3, user3Id, josieDoe3Registered.UserName),
            })));
        }

    }
}
