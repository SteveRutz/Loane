using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using KnockoutMVC.NHib.Entities;

namespace KnockoutMVC.Controllers
{
    /// <summary>
    /// Student Api controller
    /// </summary>
    /// 
   [RoutePrefix("api/truck")]
    public class TruckListController : ApiController
    {

        // GET api/student
        
        //public IEnumerable<SQLiteNHibernate.NHib.Entities.Customer> Get()
        [HttpGet]
        public string[] getTruckList()
        {
            //return new SQLiteNHibernate.NHib.DataAccess().GetCustomers();
           return TruckListRepository.GetTruckList();
        }

        [HttpPost]
        public HttpResponseMessage zaddTruck([FromBody] string truckName )
        {
            try
            {
                //string truckName = "";
                TruckListRepository.AddTruck(truckName);
                var response = Request.CreateResponse(HttpStatusCode.OK, truckName);
                return response;
            }
            catch (Exception Ex)
            {
                var response = Request.CreateResponse(HttpStatusCode.InternalServerError, "addTruck: " + Ex.Message + Ex.InnerException);
                return response;
            }

        }


        // DELETE api/student/5
       [HttpPost]
       [Route("{Truck}/{type}")]
        public HttpResponseMessage Post(string Truck, string type)
        {
            try
            {

                if(type=="add"){TruckListRepository.AddTruck(Truck);}
                else if (type == "delete") { TruckListRepository.DeleteTruck(Truck); }
                
                var response = Request.CreateResponse(HttpStatusCode.OK, Truck);
                return response;
            }
            catch (Exception Ex)
            {
                var response = Request.CreateResponse(HttpStatusCode.InternalServerError, "addTruck: " + Ex.Message + Ex.InnerException);
                return response;
            }
        }
    }
}
