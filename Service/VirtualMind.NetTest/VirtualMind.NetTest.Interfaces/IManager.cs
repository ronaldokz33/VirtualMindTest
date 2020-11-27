using System;
using System.Collections.Generic;
using System.Text;

namespace VirtualMind.NetTest.Interfaces
{
    public interface IManager<T> where T : class
    {
        IList<T> ListForLookup(T pObject);
        T Insert(T pObject);
    }
}
