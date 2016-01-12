using System;
using Lending.Cqrs.Command;

namespace Lending.Domain.OpenLibrary
{
    public class OpenLibrary : AuthenticatedCommand
    {
        public string Name { get; set; }
        public Guid AdministratorId { get; set; }

        public OpenLibrary(Guid processId, Guid userId, Guid newLibraryId, string name, Guid administratorId)
            : base(processId, newLibraryId, userId)
        {
            Name = name;
            AdministratorId = administratorId;
        }

    }
}
