using IMSCore.Infra.Aspects.Contracts;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace IMSCore.Infra.Aspects.Caching
{
    public class Cache : ICache
    {
        private static CacheConfiguration cacheConfig;
        private static readonly Lazy<ConnectionMultiplexer> lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
            ConnectionMultiplexer.Connect(cacheConfig.ConnectionString)
        );

        private static IDatabase database {
            get {
                return lazyConnection.Value.GetDatabase();
            }
        }

        public Cache(CacheConfiguration config) {
            cacheConfig = config;
        }
        public void Add(object t, string key)
        {
            var binary = BinaryCompact(t);
            bool ret = database.StringSet(key, binary);
        }       

        public void Delete(string key)
        {
            bool ret = database.KeyDelete(key);
        }

        public object Get(string key)
        {
            byte[] cachedBuffer = database.StringGet(key);
            if (cachedBuffer != null) {
                MemoryStream memoryBuffer = null;
                try
                {
                    memoryBuffer = new MemoryStream(cachedBuffer);
                    IFormatter binaryFormatter = new BinaryFormatter();
                    memoryBuffer.Position = 0;
                    var data = binaryFormatter.Deserialize(memoryBuffer);
                    Console.WriteLine("Found item in cache");
                    return data;
                }
                finally {
                    if (memoryBuffer != null) {
                        memoryBuffer.Dispose();
                    }
                }
            }
            Console.WriteLine("item Not in cache");
            return null;
        }

        private byte[] BinaryCompact(object t)
        {
            byte[] buffer = null;
            if (t != null)
            {
                IFormatter binaryFormatter = new BinaryFormatter();
                MemoryStream memoryBuffer = null;
                try
                {
                    memoryBuffer = new MemoryStream();
                    binaryFormatter.Serialize(memoryBuffer, t);
                    buffer = memoryBuffer.GetBuffer();
                    memoryBuffer.Close();
                }
                catch (Exception ex) {
                    if (memoryBuffer != null) {
                        memoryBuffer.Dispose();
                    }
                    throw ex;
                }                
            }
            return buffer;
        }
    }
}
