using Joshilewis.Infrastructure.DI;
using StructureMap;

namespace Joshilewis.Testing.Helpers
{
    public static class DIExtensions
    {
        private static IContainer container;

        public static void SetUpDependcyProvision<TContainer>(Registry registry) where TContainer : Container, new()
        {
            container = IoC.Initialize<TContainer>(registry);
        }

        public static IContainer Container => container;
    }
}
