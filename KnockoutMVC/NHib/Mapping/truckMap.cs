using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KnockoutMVC.NHib.Entities;
using FluentNHibernate.Mapping;

namespace KnockoutMVC.NHib.Mapping
{

    public class truckMap : ClassMap<truck>
    {
        public truckMap()
        {
            Id(x => x.id);
            Map(x => x.name);

        }
    }
}

