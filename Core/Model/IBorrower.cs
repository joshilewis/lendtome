using System;

namespace Core.Model
{
    public interface IBorrower
    {
        Guid Id { get; }
        string UserName { get; }
    }
}