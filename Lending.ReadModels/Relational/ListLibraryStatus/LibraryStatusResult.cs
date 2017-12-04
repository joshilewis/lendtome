using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lending.ReadModels.Relational.ListLibrayLinks;

namespace Lending.ReadModels.Relational.ListLibraryStatus
{
    public class LibraryStatusResult
    {
        public LibrarySearchResult[] ConnectedLibraries { get; set; }
        public LibrarySearchResult[] ReceivedRequests { get; set; }
        public LibrarySearchResult[] SentRequests { get; set; }

        public LibraryStatusResult(LibrarySearchResult[] connectedLibraries, LibrarySearchResult[] receivedRequests,
            LibrarySearchResult[] sentRequests)
        {
            ConnectedLibraries = connectedLibraries;
            ReceivedRequests = receivedRequests;
            SentRequests = sentRequests;
        }
    }
}
