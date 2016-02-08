using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using KnockoutMVC.NHib.Entities;
using KnockoutMVC.NHib;

namespace KnockoutMVC.Controllers
{
    /// <summary>
    /// Student Api controller
    /// </summary>
    
    [RoutePrefix("api/inventory")]
    public class InventoryController : ApiController
    {

        public IEnumerable<inventory> Get()
        {

            //System.Web.Script.Serialization.JavaScriptSerializer JSS = new System.Web.Script.Serialization.JavaScriptSerializer();
            IList<inventory> Inventory = InventoryRepository.GetInventory();
            return Inventory;
        }

        [Route("{masterItem}")]
        public IEnumerable<inventory> Get(string masterItem)
        {
            //System.Web.Script.Serialization.JavaScriptSerializer JSS = new System.Web.Script.Serialization.JavaScriptSerializer();
            IList<inventory> Inventory = InventoryRepository.GetInventory(masterItem);
            return Inventory;
        }

        // Deletes inventory...
        [Route("{id:int}")]
        public HttpResponseMessage Get(int id)
        {
            //Not able to get one record for delete.
            //Could setup a delete controller or add method to string?
            InventoryRepository.DeleteInventoryItem(id);
            var response = Request.CreateResponse(HttpStatusCode.OK, id);
            return response;

        }

        // POST api/inventory
        [Route("{masterItem}")]
        public System.Web.Mvc.ActionResult Post(List<inventory> Inventory, string masterItem)
        {

            string msg = InventoryRepository.saveAll(Inventory, masterItem);

            return new System.Web.Mvc.JsonResult()
        {
            Data = msg
            //JsonRequestBehavior = System.Web.Mvc.JsonRequestBehavior.AllowGet
        };

            //var response = Request.CreateResponse(HttpStatusCode.Created, msg);
            //string url = Url.Link("DefaultApi",new{msg});
            //response.Headers.Location = new Uri(url);
            //return response;
                
        }

        // DELETE api/student/5
        public HttpResponseMessage Delete(int id)
        {
            //EventsAccess A = new EventsAccess();
            //A.DeleteEvent(id);
            //new EventsAccess().DeleteEvent(id);
            EventsRepository.DeleteEvent(id);
            var response = Request.CreateResponse(HttpStatusCode.OK, id);
            return response;
        }
    }
}
