using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate;
using NHibernate;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Automapping;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using NHibernate.Criterion;
using System.IO;
using FluentNHibernate.Mapping;
using System.Web;

namespace KnockoutMVC
{

    static public class FluentNHibernate
    {
        //static private string DbFile = @"C:\Users\user\Documents\Visual Studio 2013\Projects\LoaneFiles\KnockoutMVC1\KnockoutMVC\bin\LoaneBrothers.db";

        static string m_DbFile;

        public static string DbFile
        {
            get
            {
                HttpContext CTX = HttpContext.Current;
                
                string m_DbFile = CTX.Server.MapPath("\\") + @"/App_Data/LoaneBrothers.db";

                if (CTX.Request.Url.ToString().IndexOf("localhost") == -1)
                {
                    m_DbFile = @"\\" + CTX.Server.MapPath("\\") + @"/LoaneBros/App_Data/LoaneBrothers.db";
                    //m_DbFile = @"\\" + CTX.Server.MapPath("\\") + @"\LoaneBrosData\LoaneBrothers.db";
                    //m_DbFile = @"\\nas-002\Winspace\10-steverutz.com\www\LoaneBrosData\LoaneBrothers.db";
                    //m_DbFile = @"\\\\nas-002\Winspace\10-steverutz.com\www\LoaneBrosData\LoaneBrothers.db";
                    //DbFile = System.Configuration.ConfigurationManager.ConnectionStrings["SqLiteCon"].ConnectionString;
                }

                return m_DbFile;
            }


            set { }
        }

        public static ISessionFactory CreateSessionFactory()
        {
        
            try
            {
                HttpContext CTX = HttpContext.Current;
                
                return Fluently.Configure()
                    //.Database(SQLiteConfiguration.Standard.InMemory)
                    .Database(SQLiteConfiguration.Standard
                     .UsingFile(DbFile))
                      // .Database(SQLiteConfiguration.Standard.ConnectionString(x => x.FromConnectionStringWithKey("mySql")))
                    .Mappings(m =>
                        m.FluentMappings.AddFromAssemblyOf<EventsRepository>())
                    //m.FluentMappings.AddFromAssembly(System.Reflection.Assembly.GetExecutingAssembly()))
                      // .ExposeConfiguration(cfg => { new SchemaExport(cfg).Execute(false, false, false); })
                    //.ExposeConfiguration(BuildSchema)
                    .BuildSessionFactory();
            }
            catch (Exception ex) { throw new Exception("File: " + DbFile + " :: " + ex.ToString()); }
        }

        private static void BuildSchema(Configuration config)
        {
            // delete the existing db on each run
            if (File.Exists(DbFile))
               // return; // Don't delete file.
            //Console.WriteLine("Delete / Recreate File");
                File.Delete(DbFile);

            // this NHibernate tool takes a configuration (with mapping info in)
            // and exports a database schema from it
            new SchemaExport(config)
                .Create(false, true);
        }

    }



}
