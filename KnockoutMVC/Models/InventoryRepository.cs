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
    public class InventoryRepository
    {
        /*
        static public IList<order> GetItemList(int id)
        {

        }
         */

        static public IList<inventory> GetInventory()
        {

            // create our NHibernate session factory
            var sessionFactory = FluentNHibernate.CreateSessionFactory();

            using (var session = sessionFactory.OpenSession())
            {
                // retreive all stores and display them
                using (session.BeginTransaction())
                {

                        IList<inventory> Inventory =  session.CreateCriteria(typeof(inventory)).List<inventory>();

                        return (from I in Inventory
                                orderby I.item
                                        select I).ToList();

                }

            }


        }

        static public inventory GetInventoryItem(int ItemId)
        {

            // create our NHibernate session factory
            var sessionFactory = FluentNHibernate.CreateSessionFactory();

            using (var session = sessionFactory.OpenSession())
            {
                // retreive all stores and display them
                using (session.BeginTransaction())
                {

                    var Inventory = from myItem in session.QueryOver<inventory>()
                                          where myItem.id == ItemId
                                          select myItem;

                    return Inventory.SingleOrDefault<inventory>();

                }

            }

        }

        static public void save(ref inventory Item)
        {

            // create our NHibernate session factory
            var sessionFactory = FluentNHibernate.CreateSessionFactory();

            using (var session = sessionFactory.OpenSession())
            {
                // retreive all stores and display them
                using (session.BeginTransaction())
                {

                    session.Save(Item);

                    session.Transaction.Commit();

                }

            }

        }

        static public string saveAll(List<inventory> Inventory)
        {

            // create our NHibernate session factory
            var sessionFactory = FluentNHibernate.CreateSessionFactory();

            using (var session = sessionFactory.OpenSession())
            {
                // retreive all stores and display them
                try{

                
                    using (session.BeginTransaction())
                    {
                        foreach (inventory item in Inventory)
                        {
                            session.SaveOrUpdate(item);
                        }


                        session.Transaction.Commit();

                        return "Inventory saved. ";

                    }

                }
                catch(Exception ex)
                {
                    return "Inventory update FAILURE. " + ex.Message + ex.InnerException;
                }

            }

        }

        internal static void InsertItem(inventory Item)
        {
            // create our NHibernate session factory
            var sessionFactory = FluentNHibernate.CreateSessionFactory();

            using (var session = sessionFactory.OpenSession())
            {

                // retreive all stores and display them
                using (var transaction = session.BeginTransaction())
                {

                    session.SaveOrUpdate(Item);

                    transaction.Commit();


                }

            }
        }


        internal static void DeleteInventoryItem(int itemId)
        {
            // create our NHibernate session factory
            var sessionFactory = FluentNHibernate.CreateSessionFactory();

            using (var session = sessionFactory.OpenSession())
            {

                // retreive all stores and display them
                using (var transaction = session.BeginTransaction())
                {
                    var queryString = string.Format("delete {0} where id = :id", typeof(inventory));
                    session.CreateQuery(queryString)
                           .SetParameter("id", itemId)
                           .ExecuteUpdate();

                    transaction.Commit();

                }

            }
        }
    }
}
