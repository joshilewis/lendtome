using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lending.Domain.Model
{
    public class AdministratorId
    {
        public AdministratorId(Guid id)
        {
            this.Id = id;
        }

        public Guid Id { get; }

        public override string ToString()
        {
            return $"Administrator with Id {Id}";
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var other = (AdministratorId)obj;
            return Id.Equals(other.Id);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
