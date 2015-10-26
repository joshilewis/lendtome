using Lending.Cqrs.Query;

namespace Tests
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