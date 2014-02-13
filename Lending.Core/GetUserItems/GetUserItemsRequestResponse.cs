using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Lending.Core.Model;

namespace Lending.Core.GetUserItems
{
    public class GetUserItemsRequestResponse
    {
        public UserOwnership[] UserOwnerships { get; private set; }

        public GetUserItemsRequestResponse(IEnumerable<Ownership<User>> payload)
        {
            UserOwnerships = payload
                .Select(x => new UserOwnership(x.Id, x.Item, x.Owner))
                .ToArray()
                ;
        }

    }

    public class UserOwnership : Ownership<User>
    {
        public UserOwnership(Guid id, Item item, User user)
            : base(item, user)
        {
            Id = id;
        }

        public override Guid Id { get; protected set; }

        [IgnoreDataMember]
        public override User Owner { get; protected set; }

        [IgnoreDataMember]
        public override int OwnerId
        {
            get { return base.OwnerId; }
        }
    }
}
