using System;
using Core.Model;

namespace Core.AddItem
{
    public class AddOrganisationItemRequest : AddItemRequest<Organisation>
    {
        public override Guid OwnerId { get; set; }

    }
}