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
using AutoMapper;

namespace KnockoutMVC
{
    public class EventsRepository
    {
        /*
        static public IList<events> GetEvents()
        {

            // create our NHibernate session factory
            var sessionFactory = FluentNHibernate.CreateSessionFactory();

            using (var session = sessionFactory.OpenSession())
            {
                // retreive all stores and display them
                using (session.BeginTransaction())
                {

                        IList<events> evts = session.CreateCriteria(typeof(events)).List<events>();

                        IList<inventory> inv = session.CreateCriteria(typeof(inventory)).List<inventory>();

                        return evts;
                                  
                }
            }


        }
        */
        //static public IList<eventItemCount> GetEvents()
        static public IList<events> GetEvents(DateTime AsOf)
        {

            //InventoryAvailability.Set();

            var sessionFactory = FluentNHibernate.CreateSessionFactory();

            using (var session = sessionFactory.OpenSession())
            {
                // retreive all stores and display them
                using (session.BeginTransaction())
                {

                    IList<events> evts = session.CreateCriteria(typeof(events))
                        .Add(NHibernate.Criterion.Restrictions.Ge("eventDate", AsOf))
                        .List<events>();

                    try
                    {
                        Mapper.CreateMap<events, events>();
                        IList<events> evtList = Mapper.Map<IList<events>, IList<events>>(evts);

                       return evtList;
                        //Mapper.CreateMap<events, eventItemCount>();
                        //IList<eventItemCount> evtListCnt = Mapper.Map<IList<events>, IList<eventItemCount>>(evts);

                        //return evtListCnt;
                    }
                    catch (Exception Ex) { string exc = Ex.ToString(); }

                    return evts;

                }
            }

            /*
            SQLiteCommand cmd = new SQLiteCommand(new SQLiteConnection());

            cmd.Connection.ConnectionString = string.Format("data source={0}", FluentNHibernate.DbFile);

            cmd.CommandText = "select "
+ " (Select count(*) from [order] where [order].orderEvent_id = events.id) As Items"
+ ", events.* "
+ " from events ";

            cmd.Connection.Open();

            SQLiteDataReader rdr = cmd.ExecuteReader();

            IList<eventItemCount> evtsItem = new List<eventItemCount>();

            while (rdr.Read())
            {
                evtsItem.Add(
                    new eventItemCount
                    {
                        available = Int32.Parse(rdr["available"].ToString())
                       ,
                        booked = Convert.ToBoolean(rdr["booked"].ToString())
                       ,
                        eventDate = Convert.ToDateTime(rdr["eventDate"].ToString())
                       ,
                        eventName = rdr["eventName"].ToString()
                       ,
                        OrderCnt = Convert.ToInt64(rdr["Items"].ToString())
                       ,
                        id = Convert.ToInt64(rdr["id"].ToString())

                    });
            }

            return evtsItem;
             */
        }

        static public events GetEvent(int EventId)
        {

            // create our NHibernate session factory
            var sessionFactory = FluentNHibernate.CreateSessionFactory();

            using (var session = sessionFactory.OpenSession())
            {
                // retreive all stores and display them
                using (session.BeginTransaction())
                {

                    try
                    {

                        events evt =  session.Load<events>(EventId);
                        return evt;

                    }
                    catch (Exception Ex)
                    { // for some reason doesn't take the first time!!
                      //  return session.CreateCriteria(typeof(events)).List<events>();
                        string msg = Ex.ToString();
                        return null;
                    }

                }
            }

        }

        internal static void Save(events eventOrder)
        {
            // create our NHibernate session factory
            var sessionFactory = FluentNHibernate.CreateSessionFactory();

            using (var session = sessionFactory.OpenSession())
            {

                // retreive all stores and display them
                using (var transaction = session.BeginTransaction())
                {

                    session.SaveOrUpdate(eventOrder);

                    transaction.Commit();


                }

            }
        }


        internal static void InsertEvent(ref events Event)
        {
            // create our NHibernate session factory
            var sessionFactory = FluentNHibernate.CreateSessionFactory();

            using (var session = sessionFactory.OpenSession())
            {

                // retreive all stores and display them
                using (var transaction = session.BeginTransaction())
                {

                    session.SaveOrUpdate(Event);

                    transaction.Commit();

                    //Event.id = myEvent.id;

                }

            }
        }

        internal static void DeleteEvent(int id)
        {
            // create our NHibernate session factory
            var sessionFactory = FluentNHibernate.CreateSessionFactory();

            using (var session = sessionFactory.OpenSession())
            {

                // retreive all stores and display them
                using (var transaction = session.BeginTransaction())
                {
                    var queryString = string.Format("delete {0} where id = :id", typeof(events));
                    session.CreateQuery(queryString)
                           .SetParameter("id", id)
                           .ExecuteUpdate();

                    transaction.Commit();

                }

            }
        }
    }
}
