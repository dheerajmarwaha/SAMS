using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IMSCore.Common
{
    public class Utilities
    {
        public static Stream GenerateStreamFromString(string input) {
            byte[] byteArray = Encoding.UTF8.GetBytes(input);
            return new MemoryStream(byteArray);
        }

        public static T GetInstance<T>()
        {
            var currentResolver = Infra.BootStrapper.ResolverFactory.Current;
            if (currentResolver != null) {
                return (T)currentResolver.GetService(typeof(T));
            }
            return default(T);
        }

        public static bool RetryUntilSuccessOrTimeout(Func<bool> task, TimeSpan timeSpan)
        {
            bool success = false;
            int elapsed = 0;
            while ((!success) && (elapsed < timeSpan.TotalMilliseconds))
            {
                Thread.Sleep(1000);
                elapsed += 1000;
                success = task();
            }
            return success;
        }
    }
}
