using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KnockoutMVC.NHib.Entities;
using FluentNHibernate.Mapping;

namespace KnockoutMVC.NHib.Mapping
{

    public class orderMap : ClassMap<order>
    {
        public orderMap()
        {
            Id(x => x.id);
            Map(x => x.item);
            Map(x => x.orderQty);
            Map(x => x.checkout);
            Map(x => x.checkin);
            Map(x => x.available);
            Map(x => x.truck);
            References(x => x.orderEvent).Not.LazyLoad();
        }
    }
}