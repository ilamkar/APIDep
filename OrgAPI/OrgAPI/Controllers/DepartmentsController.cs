using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrgDAL;
using System.Collections.Generic;
using System.Linq;

namespace OrgAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentsController : Controller
    {
        OrganizationDbContext dbContext;
        public DepartmentsController(OrganizationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        [HttpGet]
        public IEnumerable<Department> Get() //Ienumerable because it have list of data IAction
        {
            var Depts = dbContext.Departments.ToList();
            return Depts;
        }
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var Dept = dbContext.Departments.Where(x => x.Did == id).FirstOrDefault();
            return Ok(Dept);
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var Dept = dbContext.Departments.Where(x => x.Did == id).FirstOrDefault();
            if (Dept != null)
            {
                dbContext.Remove(Dept);
                dbContext.SaveChanges();
                return Ok(Dept);
            }
            else
            {
                return NotFound();
            }
           
         
        }

        [HttpPost]
        public IActionResult Post(Department D)
        {
            if (ModelState.IsValid)
            {
                dbContext.Add(D);
                dbContext.SaveChanges();
                return CreatedAtAction("Get", new { id = D.Did }, D);
            }
            else
            {
                return BadRequest(ModelState);
            }

        }
        [HttpPut]
        public IActionResult Put(Department D)
        {
            var Dept = dbContext.Departments.Where(x => x.Did == D.Did).AsNoTracking().FirstOrDefault();
            if (Dept != null)
            {
                if (ModelState.IsValid)
                {
                    dbContext.Update(D);
                    dbContext.SaveChanges();
                    return NoContent(); //or  Ok(D);
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
            else
            {
                return NotFound();
            }
        }
    }
}