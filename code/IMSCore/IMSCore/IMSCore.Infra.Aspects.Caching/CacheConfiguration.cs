using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMSCore.Infra.Aspects.Caching
{
    public class CacheConfiguration
    {
        public string ConnectionString {
            get {
                return ConfigurationManager.ConnectionStrings["RedisCache"].ConnectionString;
            }
        }
    }
}
