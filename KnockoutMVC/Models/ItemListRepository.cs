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
    public class ItemListRepository
    {
        /*
        static public IList<order> GetItemList(int id)
        {

        }
         */

        static public string[] GetItemList()
        {

            // create our NHibernate session factory
            var sessionFactory = FluentNHibernate.CreateSessionFactory();

            using (var session = sessionFactory.OpenSession())
            {
                // retreive all stores and display them
                using (session.BeginTransaction())
                {

                        IList<bom> parentItem =  session.CreateCriteria(typeof(bom)).List<bom>();

                        var ListItems = (from I in parentItem
                                        select I.item).Distinct().ToList();

                        //ListItems.Insert(0, "-- inventory item --");

                        //ListItems.Insert(1, "Test");

                        string[] myitems = ListItems.ToArray();

                        return myitems;

                }
            }


        }


    }
}
