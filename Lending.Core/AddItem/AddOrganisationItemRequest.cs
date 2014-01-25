using System;
using Lending.Core.Model;

namespace Lending.Core.AddItem
{
    public class AddOrganisationItemRequest : AddItemRequest<Organisation>
    {
        public override Guid OwnerId { get; set; }

    }
}