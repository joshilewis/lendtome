using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using FluentNHibernate.Utils;
using Lending.Core.Model;

namespace Lending.Core.GetUserItems
{
    [DataContract]
    public class GetUserItemsRequestResponse
    {
        [DataMember]
        public UserOwnership[] UserOwnerships { get; private set; }

        public GetUserItemsRequestResponse(IEnumerable<Ownership<User>> payload)
        {
            UserOwnerships = payload
                .Select(x => new UserOwnership(x.Id, x.Item))
                .ToArray()
                ;
        }

    }

    [DataContract(Name = "Ownership")]
    public class UserOwnership
    {
        [DataMember]
        public Guid Id { get; set; }

        [DataMember]
        public Item Item { get; set; }

        public UserOwnership(Guid id, Item item)
        {
            Id = id;
            Item = item;
        }
    }
}
