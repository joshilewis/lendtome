using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;

namespace Lending.Domain.Persistence
{
    public class AddedBookMap : ClassMap<AddedBook>
    {
        public AddedBookMap()
        {
            Id(x => x.Id).GeneratedBy.Assigned();
            Map(x => x.Title);
            Map(x => x.Author);
            Map(x => x.Isbn);
        }
    }
}
