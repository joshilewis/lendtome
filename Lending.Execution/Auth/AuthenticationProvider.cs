using System;

namespace Lending.Execution.Auth
{
    public class AuthenticationProvider
    {
        public virtual Guid Id { get; protected set; }
        public virtual string Name { get; protected set; }
        public virtual string UserId { get; protected set; }

        public AuthenticationProvider(Guid id, string name, string userId)
        {
            Id = id;
            Name = name;
            UserId = userId;
        }

        protected AuthenticationProvider() { }
    }
}