using System;
using Lending.Cqrs.Command;
using Lending.Cqrs.Query;
using Lending.Domain.RegisterUser;
using Lending.ReadModels.Relational.SearchForUser;
using NUnit.Framework;

namespace Tests.ReadModels
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
            var joshuaLewis = new RegisteredUser(1, Guid.NewGuid(), "Joshua Lewis");
            var suzaanHepburn = new RegisteredUser(2, Guid.NewGuid(), "Suzaan Hepburn");
            var josieDoe = new RegisteredUser(3, Guid.NewGuid(), "Josie Doe");
            var audreyHepburn = new RegisteredUser(4, Guid.NewGuid(), "Audrey Hepburn");

            SaveEntities(joshuaLewis, suzaanHepburn, josieDoe, audreyHepburn);
            CommitTransactionAndOpenNew();
            
            var query = new SearchForUser("Lew");
            var expectedResult = new Result<RegisteredUser[]>(new RegisteredUser[]
            {
                joshuaLewis,
            });

            var sut = new SearchForUserHandler(() => Session);
            Result actualResult = sut.Handle(query);

            ((Result<RegisteredUser[]>)actualResult).ShouldEqual(expectedResult);
        }

        /// <summary>
        /// GIVEN Users with the following names 'Joshua Lewis', 'Suzaan Hepburn', 'Joshua Doe', 'Audrey Hepburn' have Registered 
        /// WHEN I Search for Users with the search string 'lEw' 
        /// THEN 'Joshua Lewis' gets returne
        /// </summary>
        [Test]
        public void SearchingForUserWithSingleMatchWithWrongCaseShouldReturnThatUser()
        {
            var joshuaLewis = new RegisteredUser(1, Guid.NewGuid(), "Joshua Lewis");
            var suzaanHepburn = new RegisteredUser(2, Guid.NewGuid(), "Suzaan Hepburn");
            var josieDoe = new RegisteredUser(3, Guid.NewGuid(), "Josie Doe");
            var audreyHepburn = new RegisteredUser(4, Guid.NewGuid(), "Audrey Hepburn");

            SaveEntities(joshuaLewis, suzaanHepburn, josieDoe, audreyHepburn);
            CommitTransactionAndOpenNew();

            var query = new SearchForUser("lEw");
            var expectedResult = new Result<RegisteredUser[]>(new RegisteredUser[]
            {
                joshuaLewis,
            });

            var sut = new SearchForUserHandler(() => Session);
            Result actualResult = sut.Handle(query);

            ((Result<RegisteredUser[]>) actualResult).ShouldEqual(expectedResult);
        }

        /// <summary>
        /// GIVEN Users with the following names 'Joshua Lewis', 'Suzaan Hepburn', 'Joshua Doe', 'Audrey Hepburn' have Registered 
        /// WHEN I Search for Users with the search string 'Pet'
        /// THEN 'Joshua Lewis' gets returne
        /// </summary>
        [Test]
        public void SearchingForUserWithNoMatchesShouldReturnEmptyList()
        {
            var joshuaLewis = new RegisteredUser(1, Guid.NewGuid(), "Joshua Lewis");
            var suzaanHepburn = new RegisteredUser(2, Guid.NewGuid(), "Suzaan Hepburn");
            var josieDoe = new RegisteredUser(3, Guid.NewGuid(), "Josie Doe");
            var audreyHepburn = new RegisteredUser(4, Guid.NewGuid(), "Audrey Hepburn");

            SaveEntities(joshuaLewis, suzaanHepburn, josieDoe, audreyHepburn);
            CommitTransactionAndOpenNew();

            var query = new SearchForUser("Pet");
            var expectedResult = new Result<RegisteredUser[]>(new RegisteredUser[]
            {
            });

            var sut = new SearchForUserHandler(() => Session);
            Result actualResult = sut.Handle(query);

            ((Result<RegisteredUser[]>)actualResult).ShouldEqual(expectedResult);
        }

        /// <summary>
        /// GIVEN Users with the following names 'Joshua Lewis', 'Suzaan Hepburn', 'Joshua Doe', 'Audrey Hepburn' have Registered 
        /// WHEN I Search for Users with the search string 'Jos'
        /// THEN 'Joshua Lewis' gets returne
        /// </summary>
        [Test]
        public void SearchingForUserWithTwoMatchsShouldReturnTwoUsers()
        {
            var joshuaLewis = new RegisteredUser(1, Guid.NewGuid(), "Joshua Lewis");
            var suzaanHepburn = new RegisteredUser(2, Guid.NewGuid(), "Suzaan Hepburn");
            var josieDoe = new RegisteredUser(3, Guid.NewGuid(), "Josie Doe");
            var audreyHepburn = new RegisteredUser(4, Guid.NewGuid(), "Audrey Hepburn");

            SaveEntities(joshuaLewis, suzaanHepburn, josieDoe, audreyHepburn);
            CommitTransactionAndOpenNew();

            var query = new SearchForUser("Jos");
            var expectedResult = new Result<RegisteredUser[]>(new RegisteredUser[]
            {
                joshuaLewis,
                josieDoe,
            });

            var sut = new SearchForUserHandler(() => Session);
            Result actualResult = sut.Handle(query);

            ((Result<RegisteredUser[]>)actualResult).ShouldEqual(expectedResult);
        }

    }
}
