using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PortfolioAPI.Models.POCO.Puzzles
{
    public class PuzzleRequest
    {
        [Required]
        public int Round { get; set; }

        public string Answer { get; set; }
    }
}
