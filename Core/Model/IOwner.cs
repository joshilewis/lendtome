using System;

namespace Core.Model
{
    public interface IOwner
    {
        Guid Id { get; }
        string Name { get; }
    }
}