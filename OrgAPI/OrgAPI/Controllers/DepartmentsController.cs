﻿using Microsoft.AspNetCore.Mvc;
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
        public string Delete(int id)
        {
            var Dept = dbContext.Departments.Where(x => x.Did == id).FirstOrDefault();
            dbContext.Remove(Dept);
            dbContext.SaveChanges();
            return "Record Deleted Sucessfully";
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
        public string Put(Department D)
        {

            dbContext.Update(D);
            dbContext.SaveChanges();
            return "Updated Sucessfully";
        }
    }
}