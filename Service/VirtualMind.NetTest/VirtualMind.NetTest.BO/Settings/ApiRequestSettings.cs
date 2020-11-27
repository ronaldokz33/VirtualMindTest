using System;
using System.Collections.Generic;
using System.Text;

namespace VirtualMind.NetTest.BO.Settings
{
    public class ApiRequestSettings
    {
        public string baseUrl { get; set; }
        public Dictionary<string, decimal> currencys { get; set; }
        public string realRateApi { get; set; }
    }
}
