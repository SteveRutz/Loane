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

namespace KnockoutMVC
{
    public class TruckListRepository
    {
        /*
        static public IList<order> GetTruckList(int id)
        {

        }
        */

        static public string[] GetTruckList()
        {

            // create our NHibernate session factory
            var sessionFactory = FluentNHibernate.CreateSessionFactory();

            using (var session = sessionFactory.OpenSession())
            {
                // retreive all stores and display them
                using (session.BeginTransaction())
                {

                        IList<truck> Truck =  session.CreateCriteria(typeof(truck)).List<truck>();

                        var ListItems = (from I in Truck
                                        select I.name).Distinct().ToList();

                        //ListItems.Insert(0, "-- Load Truck --");

                        string[] myitems = ListItems.ToArray();

                        return myitems;

                }
            }
            
        }

        static public void save (int EventId, order newOrder)
        {

            // create our NHibernate session factory
            var sessionFactory = FluentNHibernate.CreateSessionFactory();

            using (var session = sessionFactory.OpenSession())
            {
                // retreive all stores and display them
                using (session.BeginTransaction())
                {

                        events evt = session.Load<events>(EventId);

                         evt.addOrder(newOrder);

                         session.SaveOrUpdate(evt);

                         session.Transaction.Commit();

                         newOrder.orderEvent = null; 

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
                    var queryString = string.Format("delete {0} where id = :id", typeof(order));
                    session.CreateQuery(queryString)
                           .SetParameter("id", id)
                           .ExecuteUpdate();

                    transaction.Commit();

                }

            }
        }
    }
}
