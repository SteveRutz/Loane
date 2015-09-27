using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using KnockoutMVC.NHib.Entities;
using KnockoutMVC.Models;

namespace KnockoutMVC.Controllers
{
    /// <summary>
    /// Student Api controller
    /// </summary>
    /// 

    [RoutePrefix("api/detail")]
    public class DetailController : ApiController
    {

        // GET api/student/5
        [ActionName("DefaultApi")]
        public IEnumerable<order> Get(int id)
        {

            IEnumerable<order> ord = DetailsRepository.GetDetail(id);

            //IList<order> ord = DetailsRepository.GetDetails(evt);


            
            foreach (order o in ord)
            {
                //eliminates circular reference in the JSON results.
                o.orderEvent.orderList = null;
            }
            

            //get inventory update.
            /*Not working... 
            foreach (order o in ord)
            {
                DetailsRepository.save(o);
            }
           */



            return ord;

        }

        // POST api/student
        //public HttpResponseMessage Post()
        public HttpResponseMessage Post(order ord)
        {

            DetailsRepository.save(ord);

            //If the orderEvent is not null the fucker fails !! serveral 
            // hours wasted.
            ord.orderEvent = null;

            var response = Request.CreateResponse(HttpStatusCode.Created, ord);
            string url = Url.Link("DefaultApi", new { ord.id });
            response.Headers.Location = new Uri(url);
            //response.Content.Dispose();
            return response;
        }

        // DELETE api/student/5
        [Route("{id:int}/{type}")]
        public HttpResponseMessage Get(int id, string type)
        {
            DetailsRepository.DeleteOrder(id);
            var response = Request.CreateResponse(HttpStatusCode.OK, id);
            return response;
        }
    }
}
