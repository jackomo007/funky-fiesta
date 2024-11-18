using Lil.TimeTracker.Models;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lil.TimeTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private TimeTrackerDbContext ctx;

        public ProjectController(TimeTrackerDbContext context)
        {
            ctx = context;
        }

        // GET: api/<ProjectController>
        [HttpGet]
       public async Task<IActionResult> Get()
        {
            var response = ctx.Projects.ProjectToType<Resources.Project>().AsEnumerable();
            return Ok(response);
        }

        // GET api/<ProjectController>/5
        [HttpGet("{id}")]
        [ProducesResponseType<Resources.Project>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(int id)
        {
            var dbProject = await ctx.Projects.FindAsync(id);
            if(dbProject == null){
                return NotFound();
            }

            var response = dbProject.Adapt<Resources.Project>();
            return Ok(response);
        }

       // POST api/<ProjectController>
        [HttpPost]
        [ProducesResponseType<Resources.Project>(StatusCodes.Status201Created)]
        [ProducesResponseType<ObjectResult>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ObjectResult>(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Post([FromBody] Resources.Project value)
        {
            if(!ModelState.IsValid){
                return Problem("Invalid Project resource request", statusCode:StatusCodes.Status400BadRequest);
            }
            try{
                var dbProject = value.Adapt<Models.Project>();

                await ctx.Projects.AddAsync(dbProject);
                await ctx.SaveChangesAsync();

                var response = dbProject.Adapt<Resources.Project>();

                return CreatedAtAction(nameof(Get), new {id=response.Id}, response);
            }
            catch(Exception ex){
                return Problem("Problem persisting Project resource", statusCode:StatusCodes.Status500InternalServerError);
            }
        }

       // PUT api/<ProjectController>/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType<Resources.Project>(StatusCodes.Status204NoContent)]
        [ProducesResponseType<ObjectResult>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ObjectResult>(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Put(int id, [FromBody] Resources.Project value)
        {
            if(!ModelState.IsValid){
                return Problem("Invalid Project resource request", statusCode:StatusCodes.Status400BadRequest);
            }
            try{
                var dbProject = value.Adapt<Models.Project>();

                ctx.Entry<Models.Project>(dbProject).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                await ctx.SaveChangesAsync();
                return NoContent();

            }
            catch(DbUpdateConcurrencyException dbex){
                var dbProject = ctx.Projects.Find(id);
                if(dbProject == null)
                {
                    return NotFound();
                }
                else{
                    return Problem("Problem persisting Project resource", statusCode:StatusCodes.Status500InternalServerError);
                }
            }
            catch(Exception ex){
                return Problem("Problem persisting Project resource", statusCode:StatusCodes.Status500InternalServerError);
            }
        }

        // DELETE api/<ProjectController>/5
        [HttpDelete("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType<Resources.Project>(StatusCodes.Status204NoContent)]
        [ProducesResponseType<ObjectResult>(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int id)
        {
            var dbProject = await ctx.Projects.FindAsync(id);
            if(dbProject == null)
            {
                return NotFound();
            }

            try{
                ctx.Projects.Remove(dbProject);
                await ctx.SaveChangesAsync();
                return NoContent();
            }
            catch(Exception ex){
                return Problem("Problem deleting Project resource", statusCode:StatusCodes.Status500InternalServerError);
            }
        }
    }
}
