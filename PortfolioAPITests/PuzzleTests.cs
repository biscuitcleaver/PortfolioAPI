using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Moq;
using PortfolioAPI.Controllers;
using PortfolioAPI.Logic.DI.Interfaces;
using PortfolioAPI.Logic.Puzzles;
using PortfolioAPI.Models.DTO.Puzzles;
using PortfolioAPI.Models.POCO.Puzzles;
using PortfolioAPI.Models.Settings;
using System;
using Xunit;

namespace PortfolioAPITests
{
    public class PuzzleTests
    {

        private EnigmaPuzzle testPuzzle;
        private AppSettings testSettings;
        private PuzzleRequest postRequestCorrect;

        public PuzzleTests()
        {
            testPuzzle = new EnigmaPuzzle();
            testSettings = new AppSettings
            {
                Round1 = "MAKE THE RAIN STOP",
                Round2 = "THE SPARROW FLIES AT NIGHT",
                Round3 = "A BALOO IS A BEAR",
                Round4 = "SNOW FALLS AT DAWN"
            };

            postRequestCorrect = new PuzzleRequest
            {
                Round = 1,
                Answer = "MAKE THE RAIN STOP"
            };

        }

        [Fact]
        public void PuzzleGetSuccess()
        {
            var settingsMoq = new Mock<IOptions<AppSettings>>();
            settingsMoq.Setup(x => x.Value).Returns(testSettings);
            var controller = new PuzzleController(settingsMoq.Object, testPuzzle);
            var result = controller.Get(1);


            ActionResult testResult = (ActionResult)result.Result;
            OkObjectResult okObjectResult = Assert.IsType<OkObjectResult>(testResult);
            Assert.Equal("DDMW XJK TLUE ANCD", okObjectResult.Value);

        }  
        
        [Fact]
        public void PuzzleGetFail()
        {
            var settingsMoq = new Mock<IOptions<AppSettings>>();
            settingsMoq.Setup(x => x.Value).Returns(testSettings);
            var controller = new PuzzleController(settingsMoq.Object, testPuzzle);
            var result = controller.Get(5);
            ActionResult testResult = (ActionResult)result.Result;
            BadRequestObjectResult objectResult = Assert.IsType<BadRequestObjectResult>(testResult);
        }

        [Fact]
        public void PostPuzzletest()
        {
            var settingsMoq = new Mock<IOptions<AppSettings>>();
            settingsMoq.Setup(x => x.Value).Returns(testSettings);
            var controller = new PuzzleController(settingsMoq.Object, testPuzzle);
            var result = controller.Post(postRequestCorrect);

            ActionResult testResult = (ActionResult)result.Result;
            OkObjectResult okObjectResult = Assert.IsType<OkObjectResult>(testResult);
            PuzzleResponse response = (PuzzleResponse)okObjectResult.Value;
            Assert.True(response.IsCorrect);
        }

    }
}
