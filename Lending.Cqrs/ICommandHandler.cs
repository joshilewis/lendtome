namespace Lending.Cqrs
{
    public interface ICommandHandler<in TCommand, out TResponse>
    {
        TResponse HandleCommand(TCommand command);
    }
}
