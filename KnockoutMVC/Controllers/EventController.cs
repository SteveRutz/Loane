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
    
    [RoutePrefix("api/event")]
    public class EventController : ApiController
    {
        public IEnumerable<events> Get()
        {

            //System.Web.Script.Serialization.JavaScriptSerializer JSS = new System.Web.Script.Serialization.JavaScriptSerializer();
            IList<events> evts = EventsRepository.GetEvents();
            foreach (events et in evts)
            {
                et.orderList = null;
            }

            return evts;
        }
        /*
        public IEnumerable<eventItemCount> Get()
        {

            //System.Web.Script.Serialization.JavaScriptSerializer JSS = new System.Web.Script.Serialization.JavaScriptSerializer();
            IList<eventItemCount> evts = EventsRepository.GetEvents();
            foreach (events et in evts){
                et.orderList = null; 
            }

            return evts;
        }
*/
        // GET: api/EventAsOf
        [Route("asof/{AsOf:datetime}")]
        public IEnumerable<events> Get(DateTime AsOf)
        //public IEnumerable<eventItemCount> Get(DateTime AsOf)
        {
            //IEnumerable<eventItemCount> evts = EventsRepository.GetEvents()
            IList<events> evts = EventsRepository.GetEvents()
            .Where(x => x.eventDate >= AsOf)
            .OrderBy(x => x.eventDate)
            .ToList();

            
            foreach (events et in evts)
            {
                //et.OrderCnt = et.orderList.Count;
                foreach (order o in et.orderList)
                o.orderEvent.orderList = null;
            }
            

            return evts;
        }
        
        // Deletes the event...
        [Route("{id:int}")]
        public HttpResponseMessage Get(int id)
        {
            //Not able to get one record for delete.
            //Could setup a delete controller or add method to string?
            EventsRepository.DeleteEvent(id);
            var response = Request.CreateResponse(HttpStatusCode.OK, id);
            return response;

        }

        // POST api/student
        public HttpResponseMessage Post(events Event)
        {


            string msg = "";
            if (Event.eventDate > Event.checkIn)
            {
                msg = "Check In: " + Event.checkIn.ToShortDateString() + " before event Date: " + Event.eventDate.ToShortDateString() + "?";
            }
            else if (Event.checkOut < Event.eventDate)
            {
                msg = "Check Out: " + Event.checkOut.ToShortDateString() + " after event Date: " + Event.eventDate.ToShortDateString() + "?";
            }

            if (msg.Length > 0)
            {
                var response = Request.CreateResponse(HttpStatusCode.InternalServerError, msg);
                string url = Url.Link("DefaultApi", new { msg });
                response.Headers.Location = new Uri(url);
                return response;
            }
            else
            {
                EventsRepository.InsertEvent(ref Event);
                var response = Request.CreateResponse(HttpStatusCode.Created, Event);
                string url = Url.Link("DefaultApi", new { Event.id });
                response.Headers.Location = new Uri(url);
                return response;
            }

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
