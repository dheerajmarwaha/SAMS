using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMSCore.Infra.Aspects.Contracts
{
    public interface ICache
    {
        void Add(object t, string key);

        Object Get(string key);
        Object GetString(string key);
        void Delete(string key);
    }
}
