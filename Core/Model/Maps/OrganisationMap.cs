using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using FluentNHibernate.Mapping;

namespace Core.Model.Maps
{
    public class OrganisationMap : BaseMap<Organisation>
    {
        public OrganisationMap()
        {
            Id(x => x.Id)
                .GeneratedBy.GuidComb();

            Map(x => x.Name);

            //TODO: Map list of Administrators
        }
    }
}
