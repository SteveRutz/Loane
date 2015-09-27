using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KnockoutMVC.NHib.Entities
{
    public class order
    {

        virtual public int id { get; set; }
        virtual public string item { get; set; }
        virtual public int orderQty { get; set; }
        virtual public DateTime checkout { get; set; }
        virtual public DateTime checkin { get; set; }
        virtual public events orderEvent { get; set; }
        virtual public int available { get; set; }
        virtual public string truck { get; set; }

    }


}