using System;
using System.Collections.Generic;
using System.Text;

namespace VirtualMind.NetTest.VO
{
    public class Purchase : _Base
    {
        public string userId { get; set; }
        public decimal amount { get; set; }
        public string currency { get; set; }
    }
}
