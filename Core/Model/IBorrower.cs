using System;

namespace Core.Model
{
    public interface IBorrower
    {
        Guid Id { get; }
        string Name { get; }
    }
}