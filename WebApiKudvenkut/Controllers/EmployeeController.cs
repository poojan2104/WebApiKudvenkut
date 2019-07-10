using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApiKudvenkut.Models;

namespace WebApiKudvenkut.Controllers
{
    public class EmployeeController : ApiController
    {
        test1Entities db = new test1Entities();

        [Authorize]
        public IEnumerable<Employee> Get()
        {
            return db.Employees.ToList();
        }
    }
}
