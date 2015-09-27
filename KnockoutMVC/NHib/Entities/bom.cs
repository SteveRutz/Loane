using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KnockoutMVC.NHib.Entities
{
    public class bom
    {
        virtual public int id { get; protected set; }
        virtual public string item { get; set; }
        virtual public string component { get; set; }
        virtual public int qty { get; set; }
    }
}