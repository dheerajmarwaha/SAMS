using IMSCore.Infra.Framework.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMSCore.Infra.BootStrapper
{
    public class ResolverFactory
    {
        static IResolver _resolver = null;

        internal static void SetResolver(IResolver resolver) {
            _resolver = resolver;
        }

        public static IResolver Current {
            get {
                return _resolver;
            }
        }
    }
}
