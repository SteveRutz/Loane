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
    
    [RoutePrefix("api/inventoryitem")]
    public class InventoryItemController : ApiController
    {

        public IEnumerable<inventory> Get()
        {

            //System.Web.Script.Serialization.JavaScriptSerializer JSS = new System.Web.Script.Serialization.JavaScriptSerializer();
            IList<inventory> Inventory = InventoryRepository.GetInventory();

            return Inventory;
        }

        // Deletes inventory...
        [Route("{id:int}")]
        public HttpResponseMessage Get(int id)
        {
            //Not able to get one record for delete.
            //Could setup a delete controller or add method to string?
            InventoryRepository.GetInventoryItem(id);
            var response = Request.CreateResponse(HttpStatusCode.OK, id);
            return response;

        }

        // POST api/inventory
        public HttpResponseMessage Post(inventory Item)
        {

            InventoryRepository.save(ref Item);
            var response = Request.CreateResponse(HttpStatusCode.Created, Item);
            string url = Url.Link("DefaultApi", new { Item.id });
            response.Headers.Location = new Uri(url);
            return response;

        }

    }
}
