using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KnockoutMVC.NHib.Entities
{
    public class inventory
    {

        virtual public int id { get;set; }
        virtual public bool master { get; set; }
        virtual public string item { get; set; }
        virtual public int qty { get; set; }

    }
}