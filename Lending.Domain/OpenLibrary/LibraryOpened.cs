using System;
using Joshilewis.Cqrs;

namespace Lending.Domain.OpenLibrary
{
    public class LibraryOpened : Event
    {
        public string Name { get; set; }
        public string AdministratorId { get; set; }
        public string Picture { get; set; }

        public LibraryOpened(Guid processId, Guid aggregateId, string name, string adminId, string picture = "picture1")
            : base(processId, aggregateId)
        {
            Name = name;
            AdministratorId = adminId;
            Picture = picture;
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
                   AdministratorId.Equals(other.AdministratorId) &&
                   Picture.Equals(other.Picture);
        }

        public override int GetHashCode()
        {
            int result = base.GetHashCode();
            result = (result * 397) ^ Name.GetHashCode();
            result = (result * 397) ^ AdministratorId.GetHashCode();
            return result;
        }
    }
}