using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lending.Cqrs.Command;

namespace Lending.Domain.RegisterUser
{
    public class RegisterUser : AuthenticatedCommand
    {
        public long AuthUserId { get; set; }
        public string DisplayName { get; set; }
        public string PrimaryEmail { get; set; }

        public RegisterUser(Guid processId, Guid newUserId, string displayName, string primaryEmail)
            : base(processId, newUserId, newUserId)
        {
            DisplayName = displayName;
            PrimaryEmail = primaryEmail;
        }

    }
}
