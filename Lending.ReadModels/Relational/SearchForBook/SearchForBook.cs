using System;
using Joshilewis.Cqrs.Query;

namespace Lending.ReadModels.Relational.SearchForBook
{
    public class SearchForBook : AuthenticatedQuery
    {
        public string SearchString { get; set; }

        public SearchForBook(string userId, string searchString)
            : base(userId)
        {
            SearchString = searchString;
        }

        public SearchForBook()
        {
        }
    }
}