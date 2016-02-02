using System;

namespace Lending.ReadModels.Relational.LibraryOpened
{
    public class OpenedLibrary
    {
        public virtual Guid Id { get; protected set; }
        public virtual string Name { get; protected set; }
        public virtual Guid AdministratorId { get; protected set; }

        public OpenedLibrary(Guid id, string name, Guid administratorId)
        {
            Id = id;
            Name = name;
            AdministratorId = administratorId;
        }

        protected OpenedLibrary()
        {
        }
    }
}
