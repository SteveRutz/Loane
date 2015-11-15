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
    
    [RoutePrefix("api/bom")]
    public class BOMController : ApiController
    {

        public IEnumerable<bom> Get()
        {

            //System.Web.Script.Serialization.JavaScriptSerializer JSS = new System.Web.Script.Serialization.JavaScriptSerializer();
            IList<bom> bom = bomRepository.GetBOM();

            return bom;
        }

        // Deletes...
        [Route("{id:int}")]
        public HttpResponseMessage Get(int id)
        {
            //Not able to get one record for delete.
            //Could setup a delete controller or add method to string?
            bomRepository.DeletePart(id);
            var response = Request.CreateResponse(HttpStatusCode.OK, id);
            return response;

        }

        // POST api/bom
        [HttpPost]
        public HttpResponseMessage Post(List<bom> BOM)
        {

            string msg = bomRepository.saveBOM(BOM);
            var response = Request.CreateResponse(HttpStatusCode.Created, msg);
            string url = Url.Link("DefaultApi",new{msg});
            response.Headers.Location = new Uri(url);
            return response;
                
        }

        // DELETE api/student/5
        [Route("{id:int}/{type}")]
        public HttpResponseMessage Delete(int id)
        {
            bomRepository.DeletePart(id);
            var response = Request.CreateResponse(HttpStatusCode.OK, id);
            return response;
        }
    }
}
