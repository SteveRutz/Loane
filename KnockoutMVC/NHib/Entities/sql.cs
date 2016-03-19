using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

namespace KnockoutMVC.NHib.Entities
{
    public static class sql
    {

        public static string participatingList(int componentId)
        {
            StringBuilder SB = new StringBuilder();
            SB.Append("Select item from bom where exists (");
            SB.Append(string.Format("select * from inventory where inventory.item = bom.component and id = {0})", componentId));

            return SB.ToString();
        }

        public static string bomList(string masterItem)
        {
            System.Text.StringBuilder SB = new System.Text.StringBuilder();
            SB.Append("select Inventory.id, Inventory.master, Inventory.item, Inventory.qty, ifnull(bom.qty, 0) As bomQty ");
            SB.Append(" from inventory left join bom on bom.component = inventory.item ");
            SB.Append(string.Format(" and bom.item = '{0}' ", masterItem));

            return SB.ToString();

        }

        public static string loadlistEvent(events evt)
        {

            System.Text.StringBuilder SB = new System.Text.StringBuilder();
            SB.Append("select ");
            SB.Append(" [order].truck, events.eventName, events.eventDate, [order].checkOut, [order].available As Avl ");
            SB.Append(" , bom.component, sum(bom.qty * [order].orderQty) As LoadQty ");
            SB.Append(" from events ");
            SB.Append(" inner join [order] on [order].orderEvent_id = events.id ");
            SB.Append(" inner join bom on bom.item = [order].item ");
            //SB.Append(string.Format(" where truck = '{0}' ", Truck));
            SB.Append(string.Format(" where events.id = {0} ", evt.id));
            SB.Append(" group by [order].truck, events.eventName, events.eventDate, [order].checkout, [order].available ");
            SB.Append(" , bom.component");

            return SB.ToString();

        }

        public static string loadlist(DateTime LoadDateBegin, DateTime LoadDateEnd, string Truck)
        {

            System.Text.StringBuilder SB = new System.Text.StringBuilder();
            SB.Append("select ");
            SB.Append(" [order].truck, events.eventName, events.eventDate, [order].checkOut, [order].available As Avl ");
            SB.Append(" , bom.component, sum(bom.qty * [order].orderQty) As LoadQty ");
            SB.Append(" from events ");
            SB.Append(" inner join [order] on [order].orderEvent_id = events.id ");
            SB.Append(" inner join bom on bom.item = [order].item ");
            //SB.Append(string.Format(" where truck = '{0}' ", Truck));
            SB.Append(string.Format(" where truck = '{0}' and eventDate between '{1}' and '{2}' "
                , Truck
                , LoadDateBegin.ToString("yyyy-MM-dd 00:00:00")
                , LoadDateEnd.ToString("yyyy-MM-dd 24:00:00")
                ));
            SB.Append(" group by [order].truck, events.eventName, events.eventDate, [order].checkout, [order].available ");
            SB.Append(" , bom.component");

            return SB.ToString();

        }

    }
}