using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using XCManager.Models;

namespace XCManager.Controllers.API
{
    public class RunnersController : ApiController
    {
        private ApplicationDbContext _context;

        public RunnersController()
        {
            _context = new ApplicationDbContext();
        }

        //GET api/Runners
        public IHttpActionResult GetRunners()
        {
            var runners = _context.Runners.ToList();

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
