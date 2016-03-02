using System;

namespace Lending.ReadModels.Relational.LibraryOpened
{
    public class OpenedLibrary
    {
        public virtual Guid Id { get; protected set; }
        public virtual string Name { get; protected set; }
        public virtual Guid AdministratorId { get; protected set; }
        public virtual string AdministratorPicture { get; set; }

        public OpenedLibrary(Guid id, string name, Guid administratorId, string administratorPicture)
        {
            Id = id;
            Name = name;
            AdministratorId = administratorId;
            AdministratorPicture = administratorPicture;
        }

        protected OpenedLibrary()
        {
        }
    }
}
