using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PortfolioAPI.Logic.Utilities
{
    public static class StringUtilities
    {

        public static String StringComparePercentange(String correctMsg, String scramedMsg)
        {
            String[] words = correctMsg.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            String regexString = string.Join("|", words.Select(x => @"\b" + x + @"\b"));
            Regex regex = new Regex("(" + regexString + ")");
            var matches = regex.Matches(scramedMsg);
            double matchPct = (double)matches.Count / words.Length * 100;
            return matchPct + "%";
        }
    }
}
