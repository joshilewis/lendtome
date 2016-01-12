using System;
using Lending.Cqrs;

namespace Lending.Domain.OpenLibrary
{
    public class LibraryOpened : Event
    {
        public string Name { get; set; }
        public Guid AdministratorId { get; set; }

        public LibraryOpened(Guid processId, Guid aggregateId, string name, Guid adminId)
            : base(processId, aggregateId)
        {
            Name = name;
            AdministratorId = adminId;
        }

        // override object.Equals
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            if (!base.Equals(obj)) return false;
            var other = (LibraryOpened)obj;
            return Name.Equals(other.Name) &&
                   AdministratorId.Equals(other.AdministratorId);
        }

    }
}