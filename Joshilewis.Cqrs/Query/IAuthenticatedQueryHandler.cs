namespace Joshilewis.Cqrs.Query
{
    public interface IAuthenticatedQueryHandler<in TQuery> : IQueryHandler<TQuery>
        where TQuery : Query, IAuthenticated
    {
    }
}
