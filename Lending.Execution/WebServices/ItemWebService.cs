using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lending.Core.Model;
using Lending.Execution.UnitOfWork;
using ServiceStack.ServiceInterface;

namespace Lending.Execution.WebServices
{
    public class ItemWebService : Service
    {
        private readonly IUnitOfWork unitOfWork;

        public ItemWebService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public virtual object Any(ItemRequest request)
        {
            var response = new ItemRequestResponse();

            unitOfWork.DoInTransaction(() =>
            {
                response.Item = unitOfWork.CurrentSession
                    .QueryOver<Item>()
                    .List()
                    .ToArray()
                    [0]
                    ;
            }
                );

            return response;
        }
    }

    public class ItemRequest
    {
        public string ItemId { get; set; }
    }

    public class ItemRequestResponse
    {
        public Item Item { get; set; }
    }
}
