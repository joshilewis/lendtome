using System;
using Joshilewis.Cqrs.Command;

namespace Lending.Domain.OpenLibrary
{
    public class OpenLibrary : AuthenticatedCommand
    {
        public string Name { get; set; }
        public string Picture { get; set; }

        public OpenLibrary(Guid processId, string userId, string name, string picture)
            : base(processId, Guid.Empty, userId)
        {
            Name = name;
            Picture = picture;
        }

        public OpenLibrary()
        {
        }
    }
}
