using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lending.Core;
using Lending.Core.AddItem;
using Lending.Core.AddUser;
using Lending.Core.BorrowItem;
using Lending.Core.Connect;
using Lending.Core.Model;
using Lending.Execution.DI;
using Lending.Execution.WebServices;
using ServiceStack;
using ServiceStack.WebHost.Endpoints;

namespace Shell
{
    internal class AppHost : AppHostHttpListenerBase
    {
        public AppHost()
            : base("HttpListener Self-Host", typeof(Request).Assembly, typeof(WebserviceBase<,>).Assembly) { }

        public override void Configure(Funq.Container container)
        {
            container.Adapter = new StructureMapContainerAdapter();

            Routes
                .Add<AddUserRequest>("/user/add")
                .Add<AddUserItemRequest>("/user/{OwnerId}/items/add/", "GET,POST")
                .Add<AddOrganisationItemRequest>("/org/{OwnerId}/items/add/")
                .Add<ConnectRequest>("/connection/add/{FromUserId}/{ToUserId}/")
                .Add<BorrowItemRequest>("/borrow/{OwnershipId}/{RequestorId}/")
                ;
        }
    }
}
