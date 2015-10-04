using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KnockoutMVC.Models;
using KnockoutMVC.NHib.Entities;
using NHibernate;

namespace KnockoutMVC.Models
{

    enum Available
    {
        Green, 
        Yellow,
        Red
    }

    internal class compOrdInv
    {
        internal DateTime myDate { get; set; }
        internal string item { get; set; }
        internal int InvQty { get; set; }
        internal int ordQty { get; set; }
        internal int extQty { get; set; }
        internal int avl { get; set; }
    }

    static public class InventoryAvailability
    {
        /* updates the inventory availability on the new Order */
        static public void checkInventoryAndSave(ref order newOrder, ISession session )
        {

                    IList<order> ord = session.CreateCriteria(typeof(order)).List<order>();

                    ord.Add(newOrder);

                    IList<inventory> inv = session.CreateCriteria(typeof(inventory)).List<inventory>();

                    IList<bom> bom = session.CreateCriteria(typeof(bom)).List<bom>();

                    IList<loadlist> tmp = session.CreateCriteria(typeof(loadlist)).List<loadlist>();

                    var MaxDate = ord.Max(x => x.checkin);

                    var MinDate = ord.Min(x => x.checkout);

                    DateTime dMax = Convert.ToDateTime(MaxDate);

                    DateTime dMin = Convert.ToDateTime(MinDate);

                    TimeSpan DaysDiff = dMax - dMin;

                    DateTime[] myDate = new DateTime[DaysDiff.Days];

                    myDate[0] = dMin;

                    for (int i = 1; i < DaysDiff.Days; i++)
                    {
                        myDate[i] = dMin.AddDays(i);
                    }

                    IList<compOrdInv> CompQty = (from o in ord
                                                 join b in bom on o.item equals b.item
                                                 join i in inv on b.component equals i.item
                                                 from d in myDate
                                                 where d >= o.checkout && d <= o.checkin
                                                 select new compOrdInv
                                                 {
                                                     myDate = d
                                                     ,
                                                     item = b.component
                                                     ,
                                                     InvQty = i.qty
                                                     ,
                                                     ordQty = o.orderQty
                                                     ,
                                                     extQty = b.qty * o.orderQty
                                                 })
                                        .ToList();

                    IList<compOrdInv> Short = (from s in CompQty
                                               group s by new { s.myDate, s.item, s.InvQty }
                                                   into g
                                                   where g.Key.InvQty - g.Sum(x => x.extQty) < 0
                                                   select new compOrdInv
                                                   {
                                                       myDate = g.Key.myDate
                                                       ,
                                                       item = g.Key.item
                                                       ,
                                                       InvQty = g.Key.InvQty
                                                       ,
                                                       extQty = g.Sum(x => x.extQty)
                                                       ,
                                                       avl = g.Key.InvQty - g.Sum(x => x.extQty)
                                                   }).ToList();


                    IList<order> ordsEffected = (from o in ord
                                                 join b in bom on o.item equals b.item
                                                 join s in Short on b.component equals s.item
                                                 where s.myDate >= o.checkout
                                                 && s.myDate <= o.checkin
                                                 && o.available == (int)Available.Red
                                                 select o
                                                ).ToList();

                    IList<order> ordsAvl = (ord.Except(ordsEffected)).ToList();

                    foreach (order o in ordsAvl)
                    {
                        o.available = (int)Available.Green;
                        o.orderEvent.available = (int)Available.Green;
                        session.SaveOrUpdate(o.orderEvent);
                    }

                    foreach (order o in ordsEffected)
                    {
                        o.available = (int)Available.Red;
                        o.orderEvent.available = (int)Available.Red;
                        //session.SaveOrUpdate(o);
                        session.SaveOrUpdate(o.orderEvent);
                    }


                //    session.Transaction.Commit();

                 //   session.Close();

        }

        static public void Set(int EventId)
        {
            SetInvAvailability(EventId, Available.Yellow);
            SetInvAvailability(EventId, Available.Red);
        }

        static private void SetInvAvailability(int EventId, Available avlType)
        {
            
            // create our NHibernate session factory
            var sessionFactory = FluentNHibernate.CreateSessionFactory();

            using (var session = sessionFactory.OpenSession())
            {
                // retreive all stores and display them
                using (session.BeginTransaction())
                {
                    events evt = session.Load<events>(EventId);

                    IList<order> ordList = evt.orderList.ToList();

                    if (ordList.Count == 0)
                    {
                        session.Close();
                        return;
                    }

                    var MaxDate = evt.checkIn; // ordList.Max(x => x.checkin);

                    var MinDate = evt.checkOut; // ordList.Min(x => x.checkout);

                    DateTime dMax = Convert.ToDateTime(MaxDate);

                    DateTime dMin = Convert.ToDateTime(MinDate);

                    IList<order> ord = session.CreateCriteria(typeof(order)).List<order>();

                    IList<inventory> inv = session.CreateCriteria(typeof(inventory)).List<inventory>();

                    IList<bom> bom = session.CreateCriteria(typeof(bom)).List<bom>();

                    IList<loadlist> tmp = session.CreateCriteria(typeof(loadlist)).List<loadlist>();

                    TimeSpan DaysDiff = dMin - dMax;

                    int Days = 1;

                    if (DaysDiff.Days > 0) { Days = DaysDiff.Days; }

                    DateTime[] myDate = new DateTime[Days];

                    myDate[0] = dMin;

                    for (int i = 1; i < DaysDiff.Days; i++)
                    {
                        myDate[i] = dMin.AddDays(i);
                    }

                    IList<compOrdInv> CompQty = (from o in ord
                                                 join b in bom on o.item equals b.item
                                                 join i in inv on b.component equals i.item
                                                 from d in myDate
                                                 where  d >= o.orderEvent.checkOut 
                                                        && d <= o.orderEvent.checkIn 
                                                        && avlType==Available.Yellow
                                                 || d == o.orderEvent.eventDate
                                                        && avlType == Available.Red
                                                 select new compOrdInv
                                                 {
                                                     myDate = d
                                                     ,
                                                     item = b.component
                                                     ,
                                                     InvQty = i.qty
                                                     ,
                                                     ordQty = o.orderQty
                                                     ,
                                                     extQty = b.qty * o.orderQty
                                                 })
                                        .ToList();

                    IList<compOrdInv> Short = (from s in CompQty
                                               group s by new { s.myDate, s.item, s.InvQty }
                                                   into g
                                                   where g.Key.InvQty - g.Sum(x => x.extQty) < 0
                                                   select new compOrdInv
                                                   {
                                                       myDate = g.Key.myDate
                                                       ,
                                                       item = g.Key.item
                                                       ,
                                                       InvQty = g.Key.InvQty
                                                       ,
                                                       extQty = g.Sum(x => x.extQty)
                                                       ,
                                                       avl = g.Key.InvQty - g.Sum(x => x.extQty)
                                                   }).ToList();

                    IList<order> ordAffected = (from o in ord
                                                 join b in bom on o.item equals b.item
                                                 join s in Short on b.component equals s.item
                                                 join ol in ordList on o.id equals ol.id
                                                 where s.myDate >= o.checkout
                                                 && s.myDate <= o.checkin
                                                 select o
                            ).ToList();

                    if (avlType == Available.Yellow)
                    {
                        foreach (order o in ordList)
                        {

                            o.available = (int)Available.Green;
                            o.orderEvent.available = (int)Available.Green;
                            session.SaveOrUpdate(o.orderEvent);
                        }
                    }

                    foreach (order o in ordAffected)
                    {
                        o.available = (int)avlType;
                        o.orderEvent.available = (int)avlType;
                        session.SaveOrUpdate(o);
                        session.SaveOrUpdate(o.orderEvent);
                    }


                    session.Transaction.Commit();

                    session.Close();            

                }
            }

        }

    }
}

/* Class 
 *     internal class compOrdInv
    {
        internal DateTime dateBegin { get; set; }
        internal DateTime dateEnd { get; set; }
        internal DateTime myDate { get; set; }
        internal string item { get; set; }
        internal int InvQty { get; set; }
        internal int ordQty { get; set; }
        internal int extQty { get; set; }
        internal int avl { get; set; }
    } */

/*     static public class InventoryAvailability
    {
        /* updates the inventory availability on the new Order */
/*  static public void checkInventoryAndSave( order newOrder, ISession session )
        static public void checkInventoryAndSave( order newOrder, ISession session )
        {

                    IList<order> ord = session.CreateCriteria(typeof(order)).List<order>();

                    if (newOrder.id == 0)
                    {
                        ord.Add(newOrder);
                    }


                    IList<inventory> inv = session.CreateCriteria(typeof(inventory)).List<inventory>();

                    IList<bom> bom = session.CreateCriteria(typeof(bom)).List<bom>();

                    IList<loadlist> tmp = session.CreateCriteria(typeof(loadlist)).List<loadlist>();

                    var MaxDate = ord.Max(x => x.checkin);

                    var MinDate = ord.Min(x => x.checkout);

                    DateTime dMax = Convert.ToDateTime(MaxDate);

                    DateTime dMin = Convert.ToDateTime(MinDate);

                    TimeSpan DaysDiff = dMax - dMin;

                    DateTime[] myDate = new DateTime[DaysDiff.Days];

                    myDate[0] = dMin;

                    for (int i = 1; i < DaysDiff.Days; i++)
                    {
                        myDate[i] = dMin.AddDays(i);
                    }

                    IList<compOrdInv> CompQty = (from o in ord
                                                 join b in bom on o.item equals b.item
                                                 join i in inv on b.component equals i.item
                                                 from d in myDate
                                                 where d >= o.checkout && d <= o.checkin
                                                 select new compOrdInv
                                                 {
                                                     myDate = d
                                                     ,
                                                     item = b.component
                                                     ,
                                                     InvQty = i.qty
                                                     ,
                                                     ordQty = o.orderQty
                                                     ,
                                                     extQty = b.qty * o.orderQty
                                                 })
                                        .ToList();

                    IList<compOrdInv> Short = (from s in CompQty
                                               group s by new { s.myDate, s.item, s.InvQty }
                                                   into g
                                                   where g.Key.InvQty - g.Sum(x => x.extQty) < 0
                                                   select new compOrdInv
                                                   {
                                                       myDate = g.Key.myDate
                                                       ,
                                                       item = g.Key.item
                                                       ,
                                                       InvQty = g.Key.InvQty
                                                       ,
                                                       extQty = g.Sum(x => x.extQty)
                                                       ,
                                                       avl = g.Key.InvQty - g.Sum(x => x.extQty)
                                                   }).ToList();


                    IList<order> ordsEffected = (from o in ord
                                                 join b in bom on o.item equals b.item
                                                 join s in Short on b.component equals s.item
                                                 where s.myDate >= o.checkout
                                                 && s.myDate <= o.checkin
                                                 //?? Why did I put this here?? && o.available == (int)Available.Red
                                                 select o
                                                ).ToList();

                    IList<order> ordsAvl = (ord.Except(ordsEffected)).ToList();

                    foreach (order o in ordsAvl)
                    {
                        o.available = 1;
                        o.orderEvent.available = 1;
                        session.SaveOrUpdate(o.orderEvent);
                    }

                    foreach (order o in ordsEffected)
                    {
                        o.available = 0;
                        o.orderEvent.available = 0;
                        //session.SaveOrUpdate(o);
                        session.SaveOrUpdate(o.orderEvent);
                    }


                //    session.Transaction.Commit();

                 //   session.Close();

        }
 * 
 * 
 * static public void Set()
        static public void Set()
        {

            // create our NHibernate session factory
            var sessionFactory = FluentNHibernate.CreateSessionFactory();

            using (var session = sessionFactory.OpenSession())
            {
                // retreive all stores and display them
                using (session.BeginTransaction())
                {

                    IList<order> ord = session.CreateCriteria(typeof(order)).List<order>();

                    IList<inventory> inv = session.CreateCriteria(typeof(inventory)).List<inventory>();

                    IList<bom> bom = session.CreateCriteria(typeof(bom)).List<bom>();

                    IList<loadlist> tmp = session.CreateCriteria(typeof(loadlist)).List<loadlist>();

                    var MaxDate = ord.Max(x => x.orderEvent.eventDate);

                    var MinDate = ord.Min(x => x.orderEvent.eventDate);

                    DateTime dMax = Convert.ToDateTime(MaxDate);

                    DateTime dMin = Convert.ToDateTime(MinDate);
                    //DateTime dMin = DateTime.Today;

                    TimeSpan DaysDiff = dMax - dMin;

                    DateTime[] myDate = new DateTime[DaysDiff.Days];

                    myDate[0] = dMin;

                    for (int i = 1; i < DaysDiff.Days; i++)
                    {
                        myDate[i] = dMin.AddDays(i);
                    }

                    IList<compOrdInv> CompQty_Period = (from o in ord
                                                 join b in bom on o.item equals b.item
                                                 join i in inv on b.component equals i.item
                                                 from d in myDate
                                                        where d.AddDays(-1) >= o.orderEvent.eventDate && o.orderEvent.eventDate <= d.AddDays(1)
                                                 select new compOrdInv
                                                 {
                                                     dateBegin = d.AddDays(-1)
                                                     , dateEnd = d.AddDays(1)
                                                     , item = b.component
                                                     , InvQty = i.qty
                                                     , ordQty = o.orderQty
                                                     , extQty = b.qty * o.orderQty
                                                 })
                                        .ToList();

                    IList<compOrdInv> Short_Period = (from s in CompQty_Period
                                               group s by new { s.dateBegin, s.dateEnd, s.item, s.InvQty }
                                                   into g
                                                   where g.Key.InvQty - g.Sum(x => x.extQty) < 0
                                                   select new compOrdInv
                                                   {
                                                       dateBegin = g.Key.dateBegin
                                                       , dateEnd = g.Key.dateEnd
                                                       , item = g.Key.item
                                                       , InvQty = g.Key.InvQty
                                                       , extQty = g.Sum(x => x.extQty)
                                                       , avl = g.Key.InvQty - g.Sum(x => x.extQty)
                                                   }).ToList();


                    IList<order> ordsEffected_Period = (from o in ord
                                                 join b in bom on o.item equals b.item
                                                 join s in Short_Period on b.component equals s.item
                                                 where s.dateBegin  >=  o.orderEvent.eventDate
                                                 && o.orderEvent.eventDate <= s.dateEnd
                                                 && o.available == 0
                                                 select o
                                                ).ToList();

                    IList<compOrdInv> CompQty_EvtDay = (from o in ord
                                                      join b in bom on o.item equals b.item
                                                      join i in inv on b.component equals i.item
                                                      from d in myDate
                                                      where d.ToShortDateString() == o.orderEvent.eventDate.ToShortDateString()
                                                      select new compOrdInv
                                                      {
                                                            dateBegin = o.orderEvent.eventDate
                                                          , dateEnd = o.orderEvent.eventDate
                                                          , item = b.component
                                                          , InvQty = i.qty
                                                          , ordQty = o.orderQty
                                                          , extQty = b.qty * o.orderQty
                                                      })
                                        .ToList();

                    IList<compOrdInv> Short_EvtDay = (from s in CompQty_EvtDay
                                                    group s by new { s.dateBegin, s.dateEnd, s.item, s.InvQty }
                                                        into g
                                                        where g.Key.InvQty - g.Sum(x => x.extQty) < 0
                                                        select new compOrdInv
                                                        {
                                                            dateBegin = g.Key.dateBegin
                                                            , dateEnd = g.Key.dateEnd
                                                            , item = g.Key.item
                                                            , InvQty = g.Key.InvQty
                                                            , extQty = g.Sum(x => x.extQty)
                                                            , avl = g.Key.InvQty - g.Sum(x => x.extQty)
                                                        }).ToList();


                    IList<order> ordsEffected_EvtDay = (from o in ord
                                                      join b in bom on o.item equals b.item
                                                      join s in Short_EvtDay on b.component equals s.item
                                                      where s.dateBegin == o.orderEvent.eventDate
                                                      select o
                                                ).ToList();

                    IList<order> ordsAvl = (ord.Except(ordsEffected_EvtDay).Except(ordsEffected_Period)).ToList();

                    foreach (order o in ordsAvl)
                    {
                        o.available = 1;
                        o.orderEvent.available = 1;
                        session.SaveOrUpdate(o.orderEvent);
                    }

                    foreach (order o in ordsEffected_Period)
                    {
                        o.available = 2;
                        o.orderEvent.available = 2;
                        session.SaveOrUpdate(o);
                        session.SaveOrUpdate(o.orderEvent);
                    }

                    foreach (order o in ordsEffected_EvtDay)
                    {
                        o.available = 0;
                        o.orderEvent.available = 0;
                        session.SaveOrUpdate(o);
                        session.SaveOrUpdate(o.orderEvent);
                    }

                    session.Transaction.Commit();

                    session.Close();

                   // return ordsEffected_EvtDay;

                }
            }
  }
 */