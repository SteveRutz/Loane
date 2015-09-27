using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Data.SQLite.EF6;
using System.Data.SQLite.Linq;
using NHibernate.Linq;
using KnockoutMVC.NHib.Entities;
using KnockoutMVC.Models;

namespace KnockoutMVC
{
    public class DetailsRepository
    {

        static public IList<order> GetDetail(int EventId)
        {
            // create our NHibernate session factory

            InventoryAvailability.Set(EventId);

            var sessionFactory = FluentNHibernate.CreateSessionFactory();

            using (var session = sessionFactory.OpenSession())
            {
                // retreive all stores and display them
                using (session.BeginTransaction())
                {
                        events evt = session.Load<events>(EventId);

                        IList<order> ord = evt.orderList.ToList();

                        return ord;

                }
            }

        }
/*
        static public IList<order> GetDetails()
        {

            // create our NHibernate session factory
            var sessionFactory = FluentNHibernate.CreateSessionFactory();

            using (var session = sessionFactory.OpenSession())
            {
                // retreive all stores and display them
                using (session.BeginTransaction())
                {

                    return InventoryAvailability.Set();


                        //return ord.ToList();
       
                }
            }


        }
 */

        //static public void save (int EventId, order newOrder)
        static public void save(order newOrder)
        {

            // create our NHibernate session factory
            var sessionFactory = FluentNHibernate.CreateSessionFactory();

            using (var session = sessionFactory.OpenSession())
            {
                // retreive all stores and display them
                using (session.BeginTransaction())
                {

                        events evt = session.Load<events>(newOrder.orderEvent.id);

                       // newOrder.orderEvent = null;

                         evt.addOrder(newOrder);

                         session.SaveOrUpdate(evt);

                         //InventoryAvailability.checkInventoryAndSave(newOrder, session);

                         /* Should get saved during the checkAvialbility */
                    
                         //session.SaveOrUpdate(evt);

                   //session.Save(newOrder.orderEvent);

                         session.Transaction.Commit();
                    
                         //newOrder.orderEvent = null; 
                    
                }

                // closing session causes problem of Lazy Loading?!

                //session.Close();

            }

        }

        static public void saveAll(events evt)
        {

            // create our NHibernate session factory
            var sessionFactory = FluentNHibernate.CreateSessionFactory();

            using (var session = sessionFactory.OpenSession())
            {
                // retreive all stores and display them
                using (session.BeginTransaction())
                {
                    
                    foreach (order o in evt.orderList)
                    {
                        o.checkin = evt.checkIn;
                        o.checkout = evt.checkOut;
                        o.orderEvent = evt; 
                    }
                    

                    session.SaveOrUpdate(evt);

                    /*
                    foreach (order ord in ords)
                    {
                        session.Update(ord);
                    }
                    */

                    session.Transaction.Commit();

                    InventoryAvailability.Set(evt.id);

                }

            }

        }

        static public void saveAll (List<order> ords)
        {

            // create our NHibernate session factory
            var sessionFactory = FluentNHibernate.CreateSessionFactory();

            using (var session = sessionFactory.OpenSession())
            {
                // retreive all stores and display them
                using (session.BeginTransaction())
                {
                    session.Update(ords[0].orderEvent);
                    
                    foreach (order ord in ords)
                    {
                        session.Update(ord);
                    }

                    InventoryAvailability.Set(ords[0].orderEvent.id);
                    
                    session.Transaction.Commit();
                                        
                }

            }

        }




        internal static void DeleteOrder(int id)
        {
            // create our NHibernate session factory
            var sessionFactory = FluentNHibernate.CreateSessionFactory();

            using (var session = sessionFactory.OpenSession())
            {

                // retreive all stores and display them
                using (var transaction = session.BeginTransaction())
                {

                    order ord = session.Load<order>(id);

                    int eventId = ord.orderEvent.id; 

                    var queryString = string.Format("delete {0} where id = :id", typeof(order));
                    session.CreateQuery(queryString)
                           .SetParameter("id", id)
                           .ExecuteUpdate();

                    transaction.Commit();

                    InventoryAvailability.Set(eventId);

                }

            }
        }
    }
}
