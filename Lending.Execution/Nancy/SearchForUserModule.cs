using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lending.Cqrs;
using Lending.Cqrs.Query;
using Lending.Execution.UnitOfWork;
using Lending.ReadModels.Relational.SearchForUser;

namespace Lending.Execution.Nancy
{
    public class SearchForUserModule : GetModule<SearchForUser, Result>
    {
        public SearchForUserModule(IUnitOfWork unitOfWork, IMessageHandler<SearchForUser, Result> messageHandler)
            : base(unitOfWork, messageHandler)
        {
        }

        protected override string Path => "/nancy/users/{searchstring}";
    }
}
