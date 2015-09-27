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


    
    [RoutePrefix("api/loadlist")]
    public class LoadListController : ApiController
    {

        /*
        public IList<loadlist> Get(DateTime LoadDateBegin, DateTime LoadDateEnd, string Truck)
        {
            //return new SQLiteNHibernate.NHib.DataAccess().GetCustomers();
            return LoadListRepository.GetLoadList(LoadDateBegin, LoadDateEnd, Truck);
           
           }
     */


        [Route("{id:int}")]
        public IEnumerable<order> Get(int id)
        {


            IEnumerable<order> ord = DetailsRepository.GetDetail(id);

            //IList<order> ord = DetailsRepository.GetDetails(evt);

            foreach (order o in ord)
            {
                o.orderEvent = null;
            }

            return ord;

        }

        // POST api/student
        //public HttpResponseMessage Post()
        public IList<loadlist> Post(loadparams truck)
        {

            return LoadListRepository.GetLoadList(truck.dateBegin, truck.dateEnd, truck.truck);

        }


        // DELETE api/student/5
        public HttpResponseMessage Delete(int id)
        {
            //DetailsRepository.DeleteOrder(id);
            var response = Request.CreateResponse(HttpStatusCode.OK, id);
            return response;
        }
    }
}
