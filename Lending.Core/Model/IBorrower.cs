using System;

namespace Lending.Core.Model
{
    public interface IBorrower
    {
        int Id { get; }
        string UserName { get; }
    }
}