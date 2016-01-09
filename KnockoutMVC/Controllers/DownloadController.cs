using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KnockoutMVC.Controllers
{
    public class DownloadController : Controller
    {
        // GET: Download
        public FileResult Database()
        {
            return new FilePathResult(FluentNHibernate.DbFile, "application/x-sqlite3");
        }
    }
}