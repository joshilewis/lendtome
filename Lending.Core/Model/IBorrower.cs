using System;

namespace Lending.Core.Model
{
    public interface IBorrower
    {
        Guid Id { get; }
        string UserName { get; }
    }
}