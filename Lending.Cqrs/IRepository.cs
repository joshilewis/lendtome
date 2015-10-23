using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lending.Cqrs
{
    public interface IRepository
    {
        void Save(object obj);
        T Get<T>(object identifier);
    }
}
