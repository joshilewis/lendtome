namespace Joshilewis.Cqrs
{
    public interface IRepository
    {
        void Save(object obj);
        T Get<T>(object identifier) where T : class;
    }
}
