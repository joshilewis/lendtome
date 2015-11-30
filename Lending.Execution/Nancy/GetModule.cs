using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lending.Cqrs;
using Lending.Cqrs.Query;
using Lending.Execution.UnitOfWork;
using Lending.ReadModels.Relational.SearchForUser;
using Nancy;
using Nancy.ModelBinding;

namespace Lending.Execution.Nancy
{
    public abstract class GetModule<TMessage, TResult> : NancyModule where TMessage : Message where TResult : Result
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMessageHandler<TMessage, TResult> messageHandler;
        protected abstract string Path { get; }

        protected GetModule(IUnitOfWork unitOfWork, IMessageHandler<TMessage, TResult> messageHandler)
        {
            this.unitOfWork = unitOfWork;
            this.messageHandler = messageHandler;

            Get[Path] = _ =>
            {
                TMessage request = this.Bind<TMessage>();

                Result response = default(Result);
                unitOfWork.DoInTransaction(() =>
                {
                    response = messageHandler.Handle(request);

                });

                return response;
            };
        }


    }
}
