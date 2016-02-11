using Joshilewis.Cqrs.Query;

namespace Lending.ReadModels.Relational.SearchForLibrary
{
    public class SearchForLibrary : Query
    {
        public string SearchString { get; set; }

        public SearchForLibrary(string searchString)
        {
            SearchString = searchString;
        }

        public SearchForLibrary()
        {
            SearchString = string.Empty;
        }
    }
}