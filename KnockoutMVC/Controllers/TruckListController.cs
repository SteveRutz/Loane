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
        [Route("getTruckList")]
        public string[] Get()
        {
            //return new SQLiteNHibernate.NHib.DataAccess().GetCustomers();
           return TruckListRepository.GetTruckList();
        }

       [HttpPost]
        [Route("addTruck")]
        public HttpResponseMessage addTruck(string truckName)
        {
            try
            {
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
        [Route("remove")]
        public HttpResponseMessage remove(string Truck)
        {
            //DetailsRepository.DeleteOrder(id);
            var response = Request.CreateResponse(HttpStatusCode.OK, Truck);
            return response;
        }
    }
}
