using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

using FluentNHibernate.Mapping;

namespace Core.Model.Maps
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
