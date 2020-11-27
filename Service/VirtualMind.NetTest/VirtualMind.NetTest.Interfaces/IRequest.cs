using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace VirtualMind.NetTest.Interfaces
{
    public interface IRequest
    {
        T Post<T>(string url, object data, string token = null);

        T Get<T>(string url, string token = null);
    }
}
