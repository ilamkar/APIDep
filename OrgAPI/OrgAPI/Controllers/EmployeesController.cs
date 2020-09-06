using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OrgDAL;

namespace OrgAPI.Controllers
{
    [Authorize(Roles="Admin,User")]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly OrganizationDbContext _context;
        UserManager<IdentityUser> userManager;

        public EmployeesController(OrganizationDbContext context, UserManager<IdentityUser> _userManager)
        {
            _context = context;
             userManager = _userManager;
        }

       // GET: api/Employees
       [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees()
        {
            var Emps = await _context.Employees.Include(x => x.Department).
                Select(x => new Employee
                {
                    Empid = x.Empid,
                    Name = x.Name,
                    Did = x.Did,
                    Department = x.Department
                }).
                ToListAsync();
            return Ok(Emps);
        }

        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees()
        //{
        //    var Emps = await _context.Employees.Include(x => x.Department).ToListAsync();
        //    var jsonResults = JsonConvert.SerializeObject(Emps, Formatting.None, new JsonSerializerSettings()
        //    {
        //        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        //    }
        //       );
        //    return Ok(jsonResults);

        //}
        // GET: api/Employees/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetEmployee(int id)
        {
            var employee = await _context.Employees.FindAsync(id);

            if (employee == null)
            {
                return NotFound();
            }

            return employee;
        }

        // PUT: api/Employees/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployee(int id, Employee employee)
        {
            if (id != employee.Empid)
            {
                return BadRequest();
            }

            _context.Entry(employee).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Employees
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        
        public async Task<ActionResult<Employee>> PostEmployee(Employee employee)
        {
     //       Employee existingEmployee = await _context.Employees.SingleOrDefaultAsync(
     //m => m.Name == employee.Name);


     //       if (existingEmployee != null)
     //       {
     //           The employee already exists.
     //            Do whatever you need to do -This is just an example.
     //           ModelState.AddModelError(string.Empty, "This employee already exists.");

     //       }

            if (ModelState.IsValid)
            {
               var user = await userManager.FindByNameAsync(User.Identity.Name);
               employee.Id = user.Id;
                _context.Add(employee);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetEmployee", new { id = employee.Empid }, employee);
            }
            else
            {
                return BadRequest(ModelState);
            }


        }



       

        // DELETE: api/Employees/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Employee>> DeleteEmployee(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();

            return employee;
        }

        private bool EmployeeExists(int id)
        {
            return _context.Employees.Any(e => e.Empid == id);
        }
    }
}
