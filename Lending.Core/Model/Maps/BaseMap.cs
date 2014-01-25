using System.Configuration;
using FluentNHibernate.Mapping;

namespace Lending.Core.Model.Maps
{
    public abstract class BaseMap<T> : ClassMap<T>
    {
        protected BaseMap()
        {
            string schema = ConfigurationManager.AppSettings["lender_db_schema"];
            Schema(schema);
        }
    }
}
