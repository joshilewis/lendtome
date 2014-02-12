using System;
using Lending.Core.Model;

namespace Lending.Core.AddItem
{
    public class AddUserItemRequest : AddItemRequest<User>
    {
        public override int OwnerId { get; set; }

    }
}