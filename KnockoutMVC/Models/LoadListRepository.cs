using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SQLite;
using System.Data.SQLite.EF6;
using System.Data.SQLite.Linq;
using KnockoutMVC.NHib.Entities;


namespace KnockoutMVC
{
    public class LoadListRepository
    {
        static public IList<loadlist> GetLoadList(DateTime LoadDateBegin, DateTime LoadDateEnd, string Truck)
        {
            SQLiteCommand cmd = new SQLiteCommand(new SQLiteConnection());

            cmd.Connection.ConnectionString = string.Format("data source={0}", FluentNHibernate.DbFile);

            cmd.CommandText = sql.loadlist(LoadDateBegin, LoadDateEnd, Truck);

            cmd.Connection.Open();

            SQLiteDataReader rdr = cmd.ExecuteReader();

            IList<loadlist> loadList = new List<loadlist>();

            while (rdr.Read())
            {
                loadList.Add(
                    new loadlist
                    {
                         Avl = Convert.ToInt64(rdr["Avl"].ToString())
                       , component = rdr["component"].ToString()
                       , checkOut = Convert.ToDateTime(rdr["checkOut"].ToString())

                        , eventDate = Convert.ToDateTime(rdr["eventDate"].ToString())
                       ,  eventName = rdr["eventName"].ToString()
                       , LoadQty = Convert.ToInt64(rdr["LoadQty"].ToString())
                       , truck = rdr["truck"].ToString()

                    });
            }

            return loadList;
        }
    }
}