using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KnockoutMVC.NHib.Entities;
using FluentNHibernate.Mapping;

namespace KnockoutMVC.NHib.Mapping
{

    public class inventoryMap : ClassMap<inventory>
    {
        public inventoryMap()
        {
            Id(x => x.id);
            Map(x => x.master);
            Map(x => x.item);
            Map(x => x.qty);
        }
    }
}

