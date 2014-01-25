using System;

namespace Lending.Core.Model
{
    public class OrganisationMembership
    {
        public virtual Guid Id { get; protected set; }
        public virtual Organisation Organisation { get; protected set; }
        public virtual User Member { get; protected set; }

        protected OrganisationMembership() { }
    }
}
