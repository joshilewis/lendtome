namespace Lending.Cqrs
{
    public interface IAuthenticatedCommandHandler<in TCommand, out TResponse> : ICommandHandler<TCommand, TResponse>
        where TCommand : AuthenticatedCommand
    {
    }
}
