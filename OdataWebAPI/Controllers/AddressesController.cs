using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.OData;
using ODataWebAPI.Data;

namespace OdataWebAPI.Controllers
{
    public class AddressesController : ODataController
    {
        private EntitiesContext db;
        public AddressesController()
        {
            this.db = new EntitiesContext();
        }

        [EnableQuery]
        public IQueryable<Address> Get()
        {
            return db.Addresses;
        }

        [EnableQuery]
        public SingleResult<Address> Get([FromODataUri] int key)
        {
            IQueryable<Address> result = db.Addresses.Where(p => p.ID == key);
            return SingleResult.Create(result);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
