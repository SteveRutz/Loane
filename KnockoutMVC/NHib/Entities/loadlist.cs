using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KnockoutMVC.NHib.Entities
{
    public class loadlist
    {
        public string truck { get; set; }
        public string eventName { get; set; }
        public DateTime checkOut { get; set; }
        public DateTime eventDate { get; set; }
        public Int64 Avl { get; set; }
        public string component { get; set; }
        public Int64 LoadQty { get; set; }
    }

    public class loadparams
    {
        public string truck { get; set; }
        public DateTime dateBegin { get; set; }
        public DateTime dateEnd { get; set; }

    }

}