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
    public class CompaniesController : ODataController
    {
        private EntitiesContext db;
        public CompaniesController()
        {
            this.db = new EntitiesContext();
        }

        [EnableQuery]
        public IQueryable<Company> Get()
        {
            return db.Companies;
        }

        [EnableQuery]
        public SingleResult<Company> Get([FromODataUri] int key)
        {
            IQueryable<Company> result = db.Companies.Where(p => p.ID == key);
            return SingleResult.Create(result);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
