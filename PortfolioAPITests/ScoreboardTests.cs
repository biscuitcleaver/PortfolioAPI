using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Moq;
using PortfolioAPI.Controllers;
using PortfolioAPI.Logic.DI.Interfaces;
using PortfolioAPI.Logic.Puzzles;
using PortfolioAPI.Models.DTO.Puzzles;
using PortfolioAPI.Models.POCO.Puzzles;
using PortfolioAPI.Models.Settings;
using Xunit;
using PortfolioAPI.Logic.DB;
using PortfolioAPI.Models.POCO.Scoreboard;
using System.Collections.Generic;
using System.Linq;

namespace PortfolioAPITests
{
    public class ScoreboardTests
    {
        private DynamoLogic dynamoClient {get; set;}
        public ScoreboardTests()
        {
            dynamoClient = new DynamoLogic();
        }

        [Fact]
        public void PuzzleGetSuccess()
        {
            var controller = new ScoreboardController(dynamoClient);
            var result = controller.Get();
            OkObjectResult okObjectResult = Assert.IsType<OkObjectResult>(result.Result);

            Assert.Equal(200, okObjectResult.StatusCode);
        }
        
        [Fact]
        public void PuzzlePostSuccess()
        {
            var controller = new ScoreboardController(dynamoClient);
            var result = controller.Post("ZZZ");

            ActionResult testResult = (ActionResult)result.Result;
            OkObjectResult okObjectResult = Assert.IsType<OkObjectResult>(testResult);
            Assert.Equal(200, okObjectResult.StatusCode);

            //Post Test Cleanup
            var postResults = dynamoClient.GetLeaderBoard().Result;
            var item = postResults.FirstOrDefault(x => x.Initials == "ZZZ");
            var deletedItem = dynamoClient.DeleteLeaderBoardEntry(item.Id);
        }
    }
}
