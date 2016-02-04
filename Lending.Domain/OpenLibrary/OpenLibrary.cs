using System;
using Lending.Cqrs.Command;

namespace Lending.Domain.OpenLibrary
{
    public class OpenLibrary : AuthenticatedCommand
    {
        public string Name { get; set; }

        public OpenLibrary(Guid processId, Guid userId, Guid newLibraryId, string name)
            : base(processId, newLibraryId, userId)
        {
            Name = name;
        }

        public OpenLibrary()
        {
        }
    }
}
