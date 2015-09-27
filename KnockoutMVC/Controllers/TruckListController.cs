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

    public class TruckListController : ApiController
    {

        // GET api/student
        
        //public IEnumerable<SQLiteNHibernate.NHib.Entities.Customer> Get()
        
        public string[] Get()
        {
            //return new SQLiteNHibernate.NHib.DataAccess().GetCustomers();
           return TruckListRepository.GetTruckList();
        }
        
        // GET api/student/5
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


        // DELETE api/student/5
        public HttpResponseMessage Delete(int id)
        {
            //DetailsRepository.DeleteOrder(id);
            var response = Request.CreateResponse(HttpStatusCode.OK, id);
            return response;
        }
    }
}
