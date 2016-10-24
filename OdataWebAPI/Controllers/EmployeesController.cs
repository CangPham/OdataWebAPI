using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.OData;
using System.Web.OData.Extensions;
using Microsoft.OData.Core;
using ODataWebAPI.Data;

namespace OdataWebAPI.Controllers
{
    public class EmployeesController : ODataController
    {
        private EntitiesContext db;

        public EmployeesController()
        {
            this.db = new EntitiesContext();
        }

        [EnableQuery]
        public IQueryable<Employee> Get()
        {
            return db.Employees;
        }

        [EnableQuery]
        public SingleResult<Employee> Get([FromODataUri] int key)
        {
            var result = db.Employees.Where(p => p.ID == key);
            return SingleResult.Create(result);
        }

        //Getting Related Entities
        //odata/Employees(100)/Address
        [EnableQuery]
        public SingleResult<Address> GetAddress([FromODataUri] int key)
        {
            var result = db.Employees.Where(m => m.ID == key).Select(m => m.Address);
            return SingleResult.Create(result);
        }

        [EnableQuery]
        public SingleResult<Company> GetCompany([FromODataUri] int key)
        {
            var result = db.Employees.Where(m => m.ID == key).Select(m => m.Company);
            return SingleResult.Create(result);
        }

        public async Task<IHttpActionResult> Post(Employee employee)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            db.Employees.Add(employee);
            await db.SaveChangesAsync();
            return Created(employee);
        }

        public async Task<IHttpActionResult> Patch([FromODataUri] int key, Delta<Employee> employee)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var entity = await db.Employees.FindAsync(key);
            if (entity == null)
            {
               return NotFound();
            }
            employee.Patch(entity);
           
            await db.SaveChangesAsync();
          
            return Updated(entity);
        }
        public async Task<IHttpActionResult> Put([FromODataUri] int key, Employee update)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (!db.Employees.Any(e => e.ID == key))
            {
                throw new HttpResponseException(
                    Request.CreateErrorResponse(
                    HttpStatusCode.NotFound,
                    new ODataError
                    {
                        ErrorCode = "NotFound.",
                        Message = "Employee " + key + " not found."
                    }));
            }
            update.ID = key;
            db.Entry(update).State = EntityState.Modified;
            await db.SaveChangesAsync();
            return Updated(update);
        }

        public async Task<IHttpActionResult> Delete([FromODataUri] int key)
        {
            var employee = await db.Employees.FindAsync(key);
            if (employee == null)
            {
                return NotFound();
            }
            db.Employees.Remove(employee);
            await db.SaveChangesAsync();
            return StatusCode(HttpStatusCode.NoContent);
        }
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            db.Dispose();
        }
    }
}
