using Lil.TimeTracker.Models;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Lil.TimeTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EmployeeController : ControllerBase
    {
        private TimeTrackerDbContext ctx;

        public EmployeeController(TimeTrackerDbContext context)
        {
            ctx = context;
        }

        // GET: api/<EmployeeController>
        [HttpGet]
        [ProducesResponseType<IEnumerable<Resources.Employee>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> Get()
        {
            //TODO: add paging support
            var response = ctx.Employees.ProjectToType<Resources.Employee>().AsEnumerable();

            var lEmployee = new List<Resources.LinkedResource<Resources.Employee>>();
            foreach (var e in response)
            {
                var lEmp = new Resources.LinkedResource<Resources.Employee>(e);
                lEmp.Links.Add(new Resources.Resource("Projects", "/api/Employee/" + e.Id.ToString() + "/Projects"));
                lEmployee.Add(lEmp);
            }

            return Ok(lEmployee);
        }

        // GET api/<EmployeeController>/5
        [HttpGet("{id}")]
        [ProducesResponseType<IEnumerable<Resources.Employee>>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(int id)
        {
            var dbEmployee = await ctx.Employees.FindAsync(id);

            if (dbEmployee == null)
            {
                return NotFound();
            }

            var response = dbEmployee.Adapt<Resources.Employee>();
            var lEmployee = new Resources.LinkedResource<Resources.Employee>(response);
            lEmployee.Links.Add(new Resources.Resource("Projects", "/api/Employee/" + response.Id.ToString() + "/Projects"));
            
            return Ok(lEmployee);
        }

         // GET api/<EmployeeController>/5/Projects
        [HttpGet("{id}/Projects")]
        [ProducesResponseType<IEnumerable<Resources.Project>>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProjects(int id)
        {
            var dbEmployee = await ctx.Employees.FindAsync(id);

            if (dbEmployee == null)
            {
                return NotFound();
            }
            else {
                await ctx.Entry(dbEmployee).Collection(e => e.Projects).LoadAsync();
                var projects = new List<Resources.Project>();
                foreach (var p in dbEmployee.Projects)
                {
                    projects.Add(p.Adapt<Resources.Project>());
                }
               
                return Ok(projects);
            }

        }

        // POST api/<EmployeeController>
        [HttpPost]
        [ProducesResponseType<IEnumerable<Resources.Employee>>(StatusCodes.Status201Created)]
        [ProducesResponseType<IEnumerable<ObjectResult>>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<IEnumerable<ObjectResult>>(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Post([FromBody] Resources.Employee value)
        {
            if (!ModelState.IsValid)
            {
                return Problem("Invalid employee request", statusCode: StatusCodes.Status400BadRequest);
            }

            try
            {
                var dbEmployee = value.Adapt<Models.Employee>();

                await ctx.Employees.AddAsync(dbEmployee);
                await ctx.SaveChangesAsync();

                var response = dbEmployee?.Adapt<Resources.Employee>();

                return CreatedAtAction(nameof(Get), new { id = response.Id }, response);
            }
            catch (Exception ex)
            {

                return Problem("Problem persisting employee resource", statusCode: StatusCodes.Status500InternalServerError);

            }
        }

        // PUT api/<EmployeeController>/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType<IEnumerable<Resources.Employee>>(StatusCodes.Status201Created)]
        [ProducesResponseType<IEnumerable<ObjectResult>>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<IEnumerable<ObjectResult>>(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Put(int id, [FromBody] Resources.Employee value)
        {
            if (!ModelState.IsValid){
                return Problem("Invalid employee request", statusCode: StatusCodes.Status400BadRequest);
            }

            try
            {
                var dbEmployee = value.Adapt<Models.Employee>();

                ctx.Entry<Models.Employee>(dbEmployee).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                
                await ctx.SaveChangesAsync();
                
                return NoContent();
            }
            catch (DbUpdateConcurrencyException dbex)
            {
                var dbEmployee = ctx.Employees.Find(id);
                if (dbEmployee == null)
                {
                    return NotFound();
                } else {
                    return Problem("Problem persisting employee resource", statusCode: StatusCodes.Status500InternalServerError);
                }
            }
            catch (Exception ex){

                return Problem("Problem persisting employee resource", statusCode: StatusCodes.Status500InternalServerError);

            }
        }


        // DELETE api/<EmployeeController>/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType<IEnumerable<Resources.Employee>>(StatusCodes.Status204NoContent)]
        [ProducesResponseType<IEnumerable<ObjectResult>>(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int id)
        {
                try
                {
                    var dbEmployee = await ctx.Employees.FindAsync(id);
                    if (dbEmployee == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        ctx.Employees.Remove(dbEmployee);
                        await ctx.SaveChangesAsync();
                        return NoContent();
                    }
                }
                catch (Exception ex)
                {
                    return Problem("Problem deleting employee resource", statusCode: StatusCodes.Status500InternalServerError);
                }
        }
    }
}
