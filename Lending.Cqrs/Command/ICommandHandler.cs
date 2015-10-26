namespace Lending.Cqrs.Command
{
    public interface ICommandHandler<in TCommand, out TResponse>
    {
        TResponse HandleCommand(TCommand command);
    }
}
