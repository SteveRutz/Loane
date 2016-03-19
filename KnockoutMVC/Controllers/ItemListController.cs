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
    public class ItemListController : ApiController
    {

        // GET api/student
        
        //public IEnumerable<SQLiteNHibernate.NHib.Entities.Customer> Get()
        
        public string[] Get()
        {
            //return new SQLiteNHibernate.NHib.DataAccess().GetCustomers();
           return ItemListRepository.GetItemList();
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

        // POST api/inventory
        //public HttpResponseMessage Post(List<order> ords)
        public events Post(events evt)
        {

            //DetailsRepository.saveAll(ords);
            DetailsRepository.saveAll(evt);

            var sessionFactory = FluentNHibernate.CreateSessionFactory();

            using (var session = sessionFactory.OpenSession())
            {
                // retreive all stores and display them
                using (session.BeginTransaction())
                {
                    evt = session.Load<events>(evt.id);

                    IList<order> ord = evt.orderList.ToList();

                    foreach (order o in ord)
                    {
                        o.orderEvent = null;
                    }

                }
            }

            return evt;
            //string msg = "Items updated";
            //var response = Request.CreateResponse(HttpStatusCode.Created, evt);
            //string url = Url.Link("DefaultApi", new { evt.id });
            //response.Headers.Location = new Uri(url);
            //return response;

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
