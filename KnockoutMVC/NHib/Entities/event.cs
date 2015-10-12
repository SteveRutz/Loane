using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SQLite;

namespace KnockoutMVC.NHib.Entities
{
    public class events
    {
        public events()
        {
            orderList = new List<order>();
            comments = "";
        }
        //provide copy function?? to make more of same event.
        virtual public int id { get; set; }
        virtual public string eventName { get; set; }
        virtual public DateTime checkOut { get; set; }
        virtual public DateTime checkIn { get; set; }
        virtual public DateTime eventDate { get; set; }
        virtual public bool booked { get; set; }
        virtual public int available { get; set; }
        virtual public IList<order> orderList { get; set; }
        virtual public string comments { get; set; }

        virtual public void addOrder(order newOrder)
        {
            newOrder.orderEvent = this;
            this.orderList.Add(newOrder);
        }

        virtual public void removeOrder(order Order)
        {
            this.orderList.Remove(Order);
        }

    }

}