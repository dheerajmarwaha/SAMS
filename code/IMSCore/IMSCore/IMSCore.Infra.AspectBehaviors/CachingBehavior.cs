using IMSCore.Infra.Aspects.Contracts;
using Microsoft.Practices.Unity.InterceptionExtension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IMSCore.Infra.AspectBehaviors
{
    public class Caching<T> : IInterceptionBehavior where T : ICache
    {
        readonly T _cache;
        public Caching(T cache)
        {
            _cache = cache;
        }
        public bool WillExecute
        {
            get
            {
                return true;
            }
        }

        public IEnumerable<Type> GetRequiredInterfaces()
        {
            return new List<Type>() { typeof(IInterceptionBehavior) };
        }

        public IMethodReturn Invoke(IMethodInvocation input, GetNextInterceptionBehaviorDelegate getNext)
        {
            Object cachedData = null;
            string cacheKey = string.Empty;

            try
            {
                cacheKey = ComputeCacheKey(input);
                cachedData = _cache.Get(cacheKey);
                if (cachedData != null)
                {
                    return input.CreateMethodReturn(cachedData);
                }
            }
            catch { }

            Console.WriteLine("Cached Check before call");
            var result = getNext()(input, getNext);

            try
            {
                _cache.Add(result.ReturnValue, cacheKey);
            }
            catch { }

            return result;
        }
        /// <summary>
        ///  Cache Key is HashCode generated from the string 
        /// "{TypeName}/{MethodName}/{filteredparam1name=filteredparam1HashCode}..{{filteredparam"n"name=filteredparam"n"HashCode}
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private string ComputeCacheKey(IMethodInvocation input)
        {
            string key = string.Format("{0},{1}", input.MethodBase.DeclaringType.Name,
                                                  input.MethodBase.Name);
            ParameterInfo[] parameters = input.MethodBase.GetParameters();
            if (parameters != null) {
                string parameterHashCode = string.Empty;
                parameters.ToList().ForEach(p =>
                {
                    parameterHashCode = (input.Inputs[p.Position] == null) ? string.Empty : input.Inputs[p.Position].ToString().GetHashCode().ToString();
                    key = string.Format("{0}/{1}={2}", key, p.Name, parameterHashCode);
                });                
            }
            key = key.GetHashCode().ToString();
            return key;
        }
    }
}
