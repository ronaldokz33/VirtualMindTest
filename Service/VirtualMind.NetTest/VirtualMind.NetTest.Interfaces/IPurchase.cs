using System;
using System.Collections.Generic;
using System.Text;
using VirtualMind.NetTest.VO;

namespace VirtualMind.NetTest.Interfaces
{
    public interface IPurchase : IManager<Purchase>
    {
        Dictionary<string, decimal> GetCurrencysQuote(string currency);
    }
}
