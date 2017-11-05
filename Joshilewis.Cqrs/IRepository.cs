namespace Joshilewis.Cqrs
{
    public interface IRepository
    {
        void Save<T>(T obj) where T : class;
        T Get<T>(object identifier) where T : class;
    }
}
