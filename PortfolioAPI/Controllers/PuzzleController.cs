using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PortfolioAPI.Logic.DI.Interfaces;
using PortfolioAPI.Logic.Puzzles;
using PortfolioAPI.Logic.Utilities;
using PortfolioAPI.Models.DTO.Puzzles;
using PortfolioAPI.Models.POCO.Puzzles;
using PortfolioAPI.Models.Settings;
using PortfolioAPI.Validation.Puzzles;

namespace PortfolioAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PuzzleController : ControllerBase
    {
        private readonly AppSettings _appSettings;
        private readonly IPuzzle _puzzle;

        public PuzzleController(IOptions<AppSettings> settings, IPuzzle puzzle)
        {
            _appSettings = settings.Value;
            _puzzle = puzzle;
        }


        [HttpGet("{round}")]
        public ActionResult<String> Get(int round)
        {
            var validationErrors = PuzzleValidator.ValidatePuzzleRequest(round);
            if (validationErrors.Any())
            {
                return BadRequest(validationErrors);
            }
            //params: accept the round value - 1 - 4. 
            //4 puzzles to solve using the enigma cipher. 
            String encrypt = "";
            String plainText = "";

            switch (round)
            {
                case 1:
                    plainText = _appSettings.Round1;
                    break;
                case 2:
                    plainText = _appSettings.Round2;
                    break;
                case 3:
                    plainText = _appSettings.Round3;
                    break;
                case 4:
                    plainText = _appSettings.Round4;
                    break;
            }

            encrypt = _puzzle.Encrypt(plainText, "AAA", "AAA", new List<int> { 1, 2, 3 });
            return Ok(encrypt);
        }

        [HttpPost]
        public ActionResult<PuzzleResponse> Post(PuzzleRequest request)
        {
            PuzzleResponse response = new PuzzleResponse
            {
                IsCorrect = false
            };

            String compareTo = "";

            switch (request.Round)
            {
                case 1:
                    compareTo = _appSettings.Round1;
                    break;
                case 2:
                    compareTo = _appSettings.Round2;
                    break;
                case 3:
                    compareTo = _appSettings.Round3;
                    break;
                case 4:
                    compareTo = _appSettings.Round4;
                    break;
            }

            response.ReturnMsg = StringUtilities.StringComparePercentange(compareTo.ToLower(), request.Answer.ToLower());
            if (compareTo.ToUpper() == request.Answer.ToUpper())
            {
                response.IsCorrect = true;
            } 

            return Ok(response);
        }
    }
}