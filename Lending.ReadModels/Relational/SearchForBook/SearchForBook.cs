using System;
using Lending.Cqrs;
using Lending.Cqrs.Query;

namespace Lending.ReadModels.Relational.SearchForBook
{
    public class SearchForBook : Query, IAuthenticated
    {
        public string SearchString { get; set; }
        public Guid UserId { get; set; }

        public SearchForBook(Guid userId, string searchString)
        {
            SearchString = searchString;
            UserId = userId;
        }

        public SearchForBook()
        {
        }
    }
}