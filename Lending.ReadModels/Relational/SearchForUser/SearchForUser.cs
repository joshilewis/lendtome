using Lending.Cqrs.Query;

namespace Lending.ReadModels.Relational.SearchForUser
{
    public class SearchForUser : Query
    {
        public string SearchString { get; set; }

        public SearchForUser(string searchString)
        {
            SearchString = searchString;
        }
    }
}