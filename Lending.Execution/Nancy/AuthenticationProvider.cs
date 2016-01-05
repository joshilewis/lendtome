namespace Lending.Execution.Nancy
{
    public class AuthenticationProvider
    {
        public virtual string Name { get; protected set; }
        public virtual string Id { get; protected set; }

        public AuthenticationProvider(string name, string id)
        {
            Name = name;
            Id = id;
        }

        protected AuthenticationProvider() { }
    }
}