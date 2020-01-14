using Microsoft.Extensions.Options;
using PortfolioAPI.Models.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortfolioAPI.Logic.Configuration
{
    public class AppConfigOption : IOptions<AppSettings>
    {
        private readonly AppSettings _appConfiguration;
        public AppConfigOption(AppSettings appConfiguration)
        {
            _appConfiguration = appConfiguration;
        }

        public AppSettings Value => _appConfiguration;
    }
}
