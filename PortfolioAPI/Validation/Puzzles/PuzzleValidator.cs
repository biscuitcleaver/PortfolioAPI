using PortfolioAPI.Models.POCO.Puzzles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortfolioAPI.Validation.Puzzles
{
    public static class PuzzleValidator
    {
        public static IEnumerable<String> ValidatePuzzleRequest(int input)
        {
            List<String> errors = new List<string>();


            if(input < 1)
            {
                errors.Add("Round value too low.");
            }

            if (input > 4)
            {
                errors.Add("Round value too high.");
            }
            return errors;
        }


    }
}
