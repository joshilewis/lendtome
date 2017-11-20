using System;

namespace Joshilewis.Cqrs
{
    public interface IAuthenticated
    {
        string UserId { get; }
    }
}
