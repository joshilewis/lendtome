using Lending.Execution.DI;
using StructureMap;

namespace Tests.FixtureExtensions
{
    public static class DIExtensions
    {
        private static IContainer container;

        public static void SetUpDependcyProvision(Registry registry)
        {
            container = IoC.Initialize(registry);
        }

        public static IContainer Container => container;
    }
}
