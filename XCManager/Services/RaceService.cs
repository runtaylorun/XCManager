using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;
using XCManager.Models;
using System.Threading.Tasks;
using System.Collections;

namespace XCManager.Services
{
    public interface IRaceService
    {
        Task<IEnumerable<Race>> GetRaces();
        Race GetRace(int raceId);
        Task SaveRace(Race race);
    }

    public class RaceService : IRaceService
    {
        private readonly ApplicationDbContext _context;
        private readonly IUserServices _userService;

        public RaceService(ApplicationDbContext context, IUserServices userServices)
        {
            _context = context;
            _userService = userServices;
        }

        public async Task<IEnumerable<Race>> GetRaces()
        {
            var user = await _userService.GetUser();
            return _context.Races.Where(t => t.Team.Id == user.Team.Id).ToList();

        }

        public Race GetRace(int raceId)
        {
            return _context.Races.SingleOrDefault(r => r.Id == raceId);
        }


        public async Task SaveRace(Race race)
        {
            var user = await _userService.GetUser();
            var team = _context.Teams.SingleOrDefault(t => t.Id == user.Team.Id);
            race.Team = team;
            if (race.Id == null || race.Id == 0)
                _context.Races.Add(race);

            _context.SaveChanges();
        }
    }
}