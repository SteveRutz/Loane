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

        static public IList<inventory> GetInventory(string masterItem)
        {
            SQLiteCommand cmd = new SQLiteCommand(new SQLiteConnection());

            cmd.Connection.ConnectionString = string.Format("data source={0}", FluentNHibernate.DbFile);

            cmd.CommandText = sql.bomList(masterItem.Replace("'", "''"));

            cmd.Connection.Open();

            SQLiteDataReader rdr = cmd.ExecuteReader();

            IList<inventory> inv = new List<inventory>();

            while (rdr.Read())
            {
                inv.Add(
                    new inventory
                    {
                        id = Convert.ToInt32(rdr["id"].ToString())
                       , item = rdr["item"].ToString()
                       , master = Convert.ToBoolean(rdr["master"].ToString())
                       , qty = Convert.ToInt32(rdr["qty"].ToString())
                       , bomQty = Convert.ToInt32( rdr["bomQty"].ToString())
                       
                    });
            }

            return inv;
        }
/*
        static public IList<inventory> GetInventory(string masterItem)
        {

            // create our NHibernate session factory
            var sessionFactory = FluentNHibernate.CreateSessionFactory();

            using (var session = sessionFactory.OpenSession())
            {
                // retreive all stores and display them
                using (session.BeginTransaction())
                {

                    IList<inventory> Inventory = session.CreateCriteria(typeof(inventory)).List<inventory>();

                    IList<bom> bom = session.CreateCriteria(typeof(bom)).List<bom>();

                    IList<bom> bomItems = bom.Where(x => x.item == masterItem).ToList();

                    IList<inventory> rtnList = (from I in Inventory
                            join b in bomItems on I.item equals b.component into tt
                            from t in tt.DefaultIfEmpty()
                            select new inventory {qty=I.qty, item=I.item, master=I.master, bomQty=(t.qty==null ? 0 : t.qty)}).ToList();

                    return rtnList;

                }

            }


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

        static public string saveAll(List<inventory> Inventory, string masterItem)
        {

            // create our NHibernate session factory
            var sessionFactory = FluentNHibernate.CreateSessionFactory();

            using (var session = sessionFactory.OpenSession())
            {
                // retreive all stores and display them
                try{
                    
                    using (session.BeginTransaction())
                    {
                        IList<bom> BOM=null;
                        if (masterItem != null)
                        {
                            BOM = session.CreateCriteria(typeof(bom)).List<bom>();
                            BOM = BOM.Where(x => x.item == masterItem).ToList();
                        }

                        if(BOM!=null){

                            foreach (inventory item in Inventory)
                            {
                                bom bomItem = BOM.Where(x => x.component == item.item).FirstOrDefault();
                                if (bomItem == null && item.bomQty > 0)
                                {
                                    bomItem = new bom();
                                    bomItem.component = item.item;
                                    bomItem.qty = item.bomQty;
                                    bomItem.item = masterItem;
                                    session.Save(bomItem);
                                }
                                else if(item.bomQty>0)
                                {
                                    bomItem.qty = item.bomQty;
                                    session.SaveOrUpdate(bomItem);
                                }

                                session.SaveOrUpdate(item);

                            }

                        }
                        else
                        {
                            foreach (inventory item in Inventory)
                            {
                                session.SaveOrUpdate(item);
                            }
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
