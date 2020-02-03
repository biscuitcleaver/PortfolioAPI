using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PortfolioAPI.Validation.Scoreboard
{
    public static class ScoreboardValidator
    {
        public static IEnumerable<String> ValidateRequest(String input)
        {
            List<String> errors = new List<string>();

            if (String.IsNullOrEmpty(input))
            {
                errors.Add("Missing Initials!");
            }

            if(input.Length > 4 || input.Length < 2)
            {
                errors.Add("Initials are invalid. Try to stick to 3 or 4 letters.");
            }

            Regex alphaOnly = new Regex(@"^[A-Z]*$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            if (!alphaOnly.IsMatch(input))
            {
                errors.Add("Letters Only.");
            }

            return errors;
        }

    }
}
