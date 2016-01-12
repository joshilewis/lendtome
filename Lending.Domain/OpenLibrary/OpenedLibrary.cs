using System;

namespace Lending.Domain.OpenLibrary
{
    public class OpenedLibrary
    {
        public virtual Guid Id { get; protected set; }
        public virtual string Name { get; protected set; }

        public OpenedLibrary(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        protected OpenedLibrary()
        {
        }
    }
}
