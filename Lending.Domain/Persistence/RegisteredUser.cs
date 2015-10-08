using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lending.Domain.Persistence
{
    public class RegisteredUser
    {
        public virtual Guid Id { get; protected set; }
        public virtual string UserName { get; protected set; }

        public RegisteredUser(Guid id, string userName)
        {
            Id = id;
            UserName = userName;
        }

        protected RegisteredUser()
        {
        }
    }
}
