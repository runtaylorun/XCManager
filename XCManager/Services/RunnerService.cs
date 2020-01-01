using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using XCManager.Models;
using System.Data.Entity;

namespace XCManager.Services
{
    public interface IRunnerServices
    {
        Task<List<Runner>> GetTeamRoster();
        Runner GetRunner(int id);
        void DeleteRunner(int id);
        Task PostRunner(Runner runner);
        List<IndividualResult> GetRunnersRecentResults(int id);
        Dictionary<string, TimeSpan> GetRunnersPersonalBests(int id);
    }

    public class RunnerServices : IRunnerServices
    {
        private readonly ApplicationDbContext _context;
        private readonly IUserServices _userService;

        public RunnerServices(ApplicationDbContext context, IUserServices userService)
        {
            _context = context;
            _userService = userService;
        }

        public async Task<List<Runner>> GetTeamRoster()
        {
            var user = await _userService.GetUser();
            var list = _context.Runners.Where(r => r.Team.Id == user.Team.Id).ToList();
            return _context.Runners.Where(r => r.Team.Id == user.Team.Id).ToList();
        }

        public Runner GetRunner(int id)
        {
           return _context.Runners.SingleOrDefault(r => r.Id == id);
        }

        public void DeleteRunner(int id)
        {
            var runnerToDelete = _context.Runners.SingleOrDefault(r => r.Id == id);

            _context.Runners.Remove(runnerToDelete);
            _context.SaveChanges();
        }

        public async Task PostRunner(Runner runner)
        {
            var user = await _userService.GetUser();

            if (runner.Id == null || runner.Id == 0)
            {
                var team = _context.Teams.SingleOrDefault(t => t.Id == user.Team.Id);
                runner.Team = team;
                _context.Runners.Add(runner);
            }
            else
            {
                var runnerToUpdate = _context.Runners.SingleOrDefault(r => r.Id == runner.Id);

                runnerToUpdate.Name = runner.Name;
                runnerToUpdate.Grade = runner.Grade;
                runnerToUpdate.Email = runner.Email;
            }

            _context.SaveChanges();
        }

        public List<IndividualResult> GetRunnersRecentResults(int id)
        {
            return _context.IndividualResults.Include(r => r.Race).
                OrderByDescending(r => r.Race.Date).
                Where(r => r.Runner.Id == id).
                Take(3).ToList();
        }

        public Dictionary<string, TimeSpan> GetRunnersPersonalBests(int id)
        {
            Dictionary<string, TimeSpan> personalBests = new Dictionary<string, TimeSpan>();

            var Distances = _context.Races.Select(r => r.Distance).Distinct().ToList();

            foreach (string distance in Distances)
            {
                var bestTime = (from a in _context.IndividualResults
                                join c in _context.Races on a.Race.Id equals c.Id
                                join d in _context.Runners on a.Runner.Id equals d.Id
                                where c.Distance == distance && a.Runner.Id == id
                                orderby a.FinishingTime ascending
                                select a.FinishingTime).FirstOrDefault();

                personalBests.Add(distance, bestTime);
            }

            return personalBests;
        }

    }
}