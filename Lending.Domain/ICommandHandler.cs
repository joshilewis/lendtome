namespace Lending.Domain
{
    public interface ICommandHandler<in TCommand, out TResponse>
    {
        TResponse HandleCommand(TCommand request);
    }
}
