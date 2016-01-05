using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin.Extensions;
using Owin;
using Owin.StatelessAuth;

namespace Lending.Execution.Owin
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.RequiresStatelessAuth(new SecureTokenValidator(),
                    new StatelessAuthOptions { });
            app.UseNancy();
            app.UseStageMarker(PipelineStage.MapHandler);
        }
    }
}
