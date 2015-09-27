using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KnockoutMVC.NHib.Entities;
using FluentNHibernate.Mapping;

namespace KnockoutMVC.NHib.Mapping
{
    public class bomMap : ClassMap<bom>
    {
        public bomMap()
        {
            Id(x => x.id);
            Map(x => x.item);
            Map(x => x.component);
            Map(x => x.qty);
        }
    }
}