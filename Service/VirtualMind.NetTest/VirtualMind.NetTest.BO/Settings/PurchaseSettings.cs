using System;
using System.Collections.Generic;
using System.Text;

namespace VirtualMind.NetTest.BO.Settings
{
    public class PurchaseSettings
    {
        public Dictionary<string, decimal> limitByMonth { get; set; }
        public Dictionary<string, decimal> limitByPurchase { get; set; }
    }
}
