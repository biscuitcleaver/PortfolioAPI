using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortfolioAPI.Logic.DI.Interfaces
{
    public interface IPuzzle
    {
        public String Encrypt(String plain, String keySettings, String ringSettings, List<int> rotorSettings);
    }
}
