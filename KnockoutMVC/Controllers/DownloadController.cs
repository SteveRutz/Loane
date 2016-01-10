using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;

namespace KnockoutMVC.Controllers
{
    public class DownloadController : Controller
    {
        // GET: Download
        public HttpResponseMessage Database(string html, string WorkbookName = "test.xls")
        {

            Response.Clear();
            Response.AddHeader("Content-Type", "application/msexcel");
            Response.AddHeader("Content-Disposition", "attachment;filename=" + WorkbookName);
            Response.Flush();
            Response.Write(html);
            Response.Flush();
            Response.End();

            return null; 

            Response.ContentType = "application/x-sqlite3";
            Response.Clear();
            Response.BufferOutput = true;
            //Response.ClearContent();
            Response.AddHeader("Content-Disposition", "attachment; filename=" + FluentNHibernate.DbFile);
            //Response.AddHeader("Content-Length", file.Length.ToString());
            Response.TransmitFile(FluentNHibernate.DbFile);
            //Response.End(); Will raise that error. this works well locally but not with IIS
            Response.Flush();//Won't get error with Flush() so use this Instead of End()

            //return new FilePathResult(FluentNHibernate.DbFile, "application/x-sqlite3");
            return null;
        }
    }
}