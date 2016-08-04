using Nancy.Bootstrapper;

namespace Lending.Execution.Auth
{
    public class EnableSessions : IApplicationStartup
    {
        public void Initialize(IPipelines pipelines)
        {
            Nancy.Session.CookieBasedSessions.Enable(pipelines);
        }
    }
}