namespace Lending.Cqrs.Command
{
    public interface IAuthenticatedCommandHandler<in TCommand, out TResponse> : ICommandHandler<TCommand, TResponse>
        where TCommand : AuthenticatedCommand
    {
    }
}
