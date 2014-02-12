using System;
using Lending.Core.Model;

namespace Lending.Core.AddItem
{
    public abstract class AddItemRequest<T> where T : IOwner
    {
        public string Title { get; set; }
        public string Creator { get; set; }
        public string Edition { get; set; }
        public abstract int OwnerId {get; set; }
    }
}