using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KnockoutMVC.NHib.Entities;
using FluentNHibernate.Mapping;

namespace KnockoutMVC.NHib.Mapping
{
    public class eventsMap : ClassMap<events>
    {
        public eventsMap()
        {
            Id(x => x.id);
            Map(x => x.eventName);
            Map(x => x.eventDate);
            Map(x => x.checkOut);
            Map(x => x.checkIn);
            Map(x => x.available);
            Map(x => x.booked);
            HasMany(x => x.orderList)
            .KeyColumn("orderEvent_id")
            .Inverse()
            .Cascade.All(); 
        }
    }
}