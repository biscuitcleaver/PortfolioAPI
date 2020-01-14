using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortfolioAPI.Models.DTO.Puzzles
{
    public class PuzzleResponse
    {
        public Boolean IsCorrect { get; set; }
        public String ReturnMsg { get; set; }
    }
}
