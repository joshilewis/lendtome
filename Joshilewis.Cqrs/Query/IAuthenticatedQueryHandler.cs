namespace Joshilewis.Cqrs.Query
{
    public interface IAuthenticatedQueryHandler<in TQuery, out TResult> : IQueryHandler<TQuery, TResult>
        where TQuery : Query, IAuthenticated
    {
    }
}
