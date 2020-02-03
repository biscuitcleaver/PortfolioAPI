using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Microsoft.AspNetCore.Mvc;
using PortfolioAPI.Logic.DB;
using PortfolioAPI.Logic.DI.Interfaces;
using PortfolioAPI.Models.DTO.Scoreboards;
using PortfolioAPI.Models.POCO.Scoreboard;
using PortfolioAPI.Validation.Scoreboard;

namespace PortfolioAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ScoreboardController : ControllerBase
    {
        private readonly INoSql noSqlClient;
      
        public ScoreboardController(INoSql _noSqlClient)
        {
        
            noSqlClient = _noSqlClient;
        }

        [HttpGet]
        public ActionResult<List<ScoreBoard>> Get()
        {
            return Ok(noSqlClient.GetLeaderBoard().Result);
        }

        [HttpPost]
        public ActionResult<Boolean> Post(ScoreboardDTO request)
        {
            var validationErrors = ScoreboardValidator.ValidateRequest(request.initials);
            if (validationErrors.Any())
            {
                return BadRequest(validationErrors);
            }

            noSqlClient.AddLeaderBoardEntry(request.initials);

            return Ok(true);
        }

    }
}