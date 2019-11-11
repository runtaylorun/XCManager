using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using XCManager.Models;

namespace XCManager.Controllers.API
{
    public class RunnersController : ApiController
    {
        private ApplicationDbContext _context;
        private UserManager<ApplicationUser> UserManager { get; set; }

        public RunnersController()
        {
            _context = new ApplicationDbContext();
            UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_context));
        }

        //GET api/Runners
        public async Task<IHttpActionResult> GetRunners()
        {
            var user = await UserManager.FindByNameAsync(User.Identity.GetUserName());
            var runners = _context.Runners.Select(r => r.Team.Id == user.Team.Id).ToList();

            return Ok(runners);
        }


        //GET api/Runners/1
        public IHttpActionResult GetRunners(int id)
        {
            var runner = _context.Runners.SingleOrDefault(c => c.Id == id);

            return Ok(runner);
        }

        //POST api/Runners
        [HttpPost]
        public IHttpActionResult CreateRunner(Runner runner)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var newRunner = new Runner()
            {
                Name = runner.Name,
                Grade = runner.Grade,
                Email = runner.Email
            };
               

            _context.Runners.Add(newRunner);
            _context.SaveChanges();

            return Ok();
        }

        //PUT api/Runners/1
        [HttpPut]
        public IHttpActionResult UpdateRunner(Runner runner)
        {
            var runnerInDb = _context.Runners.SingleOrDefault(c => c.Id == runner.Id);
            if (runnerInDb == null)
                return NotFound();

            runnerInDb.Name = runner.Name;
            runnerInDb.Grade = runner.Grade;
            runnerInDb.Email = runner.Email;
            _context.SaveChanges();

            return Ok();
        }

        //DELETE api/Runners/1
        [HttpDelete]
        public IHttpActionResult DeleteRunner(int id)
        {
            var runnerToDelete = _context.Runners.SingleOrDefault(c => c.Id == id);

            _context.Runners.Remove(runnerToDelete);
            _context.SaveChanges();

            return Ok();
        }
    }
}
