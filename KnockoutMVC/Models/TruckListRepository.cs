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

        internal static void AddTruck(string Name)
        {
            // create our NHibernate session factory
            var sessionFactory = FluentNHibernate.CreateSessionFactory();

            using (var session = sessionFactory.OpenSession())
            {

                // retreive all stores and display them
                using (var transaction = session.BeginTransaction())
                {
                    truck Truck = new truck();
                    Truck.name = Name;

                    session.Save(Truck);

                    transaction.Commit();

                }

            }
        }

        internal static void DeleteTruck(string Name)
        {
            // create our NHibernate session factory
            var sessionFactory = FluentNHibernate.CreateSessionFactory();

            using (var session = sessionFactory.OpenSession())
            {

                // retreive all stores and display them
                using (var transaction = session.BeginTransaction())
                {
                    var queryString = string.Format("delete {0} where name = :name", typeof(truck));
                    session.CreateQuery(queryString)
                           .SetParameter("name", Name)
                           .ExecuteUpdate();

                    transaction.Commit();

                }

            }
        }
    }
}
