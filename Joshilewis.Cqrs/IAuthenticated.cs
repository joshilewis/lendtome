using System;

namespace Joshilewis.Cqrs
{
    public interface IAuthenticated
    {
        Guid UserId { get; }
    }
}
