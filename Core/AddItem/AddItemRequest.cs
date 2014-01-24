using System;
using Core.Model;

namespace Core.AddItem
{
    public abstract class AddItemRequest<T> where T : IOwner
    {
        public string Title { get; set; }
        public string Creator { get; set; }
        public string Edition { get; set; }
        public abstract Guid OwnerId {get; set; }
    }
}