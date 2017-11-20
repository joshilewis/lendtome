using System;
using Joshilewis.Cqrs.Command;

namespace Lending.Domain.OpenLibrary
{
    public class OpenLibrary : AuthenticatedCommand
    {
        public string Name { get; set; }

        public OpenLibrary(Guid processId, string userId, string name)
            : base(processId, Guid.Empty, userId)
        {
            Name = name;
        }

        public OpenLibrary()
        {
        }
    }
}
