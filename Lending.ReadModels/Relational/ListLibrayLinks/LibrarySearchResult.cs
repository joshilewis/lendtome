using System;

namespace Lending.ReadModels.Relational.ListLibrayLinks
{
    public class LibrarySearchResult
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public LibrarySearchResult(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        public LibrarySearchResult()
        {
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var other = (LibrarySearchResult)obj;
            return Id.Equals(other.Id) &&
                   Name.Equals(other.Name);
        }
    }
}