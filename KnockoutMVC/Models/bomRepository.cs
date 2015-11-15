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
    public class bomRepository
    {

        static public IList<bom> GetBOM()
        {

            // create our NHibernate session factory
            var sessionFactory = FluentNHibernate.CreateSessionFactory();

            using (var session = sessionFactory.OpenSession())
            {
                // retreive all stores and display them
                using (session.BeginTransaction())
                {

                        IList<bom> BOM =  session.CreateCriteria(typeof(bom)).List<bom>();

                        return (from I in BOM
                                orderby I.item
                                        select I).ToList();

                }

            }


        }

        static public void save(bom Part)
        {

            // create our NHibernate session factory
            var sessionFactory = FluentNHibernate.CreateSessionFactory();

            using (var session = sessionFactory.OpenSession())
            {
                // retreive all stores and display them
                using (session.BeginTransaction())
                {

                    session.Save(Part);

                    session.Transaction.Commit();

                }

            }

        }

        static public string saveBOM(List<bom> BOM)
        {

            // create our NHibernate session factory
            var sessionFactory = FluentNHibernate.CreateSessionFactory();

            using (var session = sessionFactory.OpenSession())
            {
                // retreive all stores and display them
                try{

                
                    using (session.BeginTransaction())
                    {
                        foreach (bom Part in BOM)
                        {
                            session.SaveOrUpdate(Part);
                        }


                        session.Transaction.Commit();

                        return "BOM saved. ";

                    }

                }
                catch(Exception ex)
                {
                    return "Inventory update FAILURE. " + ex.Message + ex.InnerException;
                }

            }

        }

        internal static void InsertPart(bom part)
        {
            // create our NHibernate session factory
            var sessionFactory = FluentNHibernate.CreateSessionFactory();

            using (var session = sessionFactory.OpenSession())
            {

                // retreive all stores and display them
                using (var transaction = session.BeginTransaction())
                {

                    session.SaveOrUpdate(part);

                    transaction.Commit();


                }

            }
        }


        internal static void DeletePart(int id)
        {
            // create our NHibernate session factory
            var sessionFactory = FluentNHibernate.CreateSessionFactory();

            using (var session = sessionFactory.OpenSession())
            {

                // retreive all stores and display them
                using (var transaction = session.BeginTransaction())
                {
                    var queryString = string.Format("delete {0} where id = :id", typeof(bom));
                    session.CreateQuery(queryString)
                           .SetParameter("id", id)
                           .ExecuteUpdate();

                    transaction.Commit();

                }

            }
        }
    }
}
