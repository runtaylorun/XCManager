using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using XCManager.Models;

namespace XCManager.Controllers
{
    public class RosterController : Controller
    {
        ApplicationDbContext _context = new ApplicationDbContext();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult NewRunner()
        {
            return View("RunnerForm");
        }

        public ActionResult UpdateRunner(int id)
        {
            var runner = _context.Runners.SingleOrDefault(r => r.Id == id);
            return View("RunnerForm", runner);
        }

        public ActionResult Save(Runner runner)
        {
            if (runner.Id == 0)
                _context.Runners.Add(runner);
            else
            {
              var runnerToUpdate = _context.Runners.SingleOrDefault(r => r.Id == runner.Id);

                runnerToUpdate.Name = runner.Name;
                runnerToUpdate.Grade = runner.Grade;
                runnerToUpdate.Email = runner.Email;
            }

            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult RunnerDetails(int id)
        {
            var runner = _context.Runners.SingleOrDefault(r => r.Id == id);

            return View(runner);
        }
    }
}