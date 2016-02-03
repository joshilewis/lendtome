using System;
using Lending.Cqrs;
using Lending.Cqrs.Query;

namespace Lending.ReadModels.Relational.SearchForBook
{
    public class SearchForBook : AuthenticatedQuery
    {
        public string SearchString { get; set; }

        public SearchForBook(Guid userId, string searchString)
            : base(userId)
        {
            SearchString = searchString;
        }

        public SearchForBook()
        {
        }
    }
}