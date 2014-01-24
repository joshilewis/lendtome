using System;
using Core.Model;

namespace Core.AddItem
{
    public class AddUserItemRequest : AddItemRequest<User>
    {
        public override Guid OwnerId { get; set; }

    }
}