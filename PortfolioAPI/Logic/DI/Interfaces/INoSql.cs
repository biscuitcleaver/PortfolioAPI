using PortfolioAPI.Models.POCO.Scoreboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortfolioAPI.Logic.DI.Interfaces
{
    public interface INoSql
    {
        public Task<List<ScoreBoard>> GetLeaderBoard();

        public Task AddLeaderBoardEntry(String initials);
    }
}
